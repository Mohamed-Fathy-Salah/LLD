using System.Collections.Concurrent;

namespace SplitWise;

public class Group(string name, IGroupObserver[] participants)
{
    public string Name { get; set; } = name;
    public ConcurrentBag<Expense> Expenses { get; } = [];
    public ConcurrentDictionary<IGroupObserver, decimal> ParticipantsPaymentDetails { get; } = new(participants.ToDictionary(f => f, f => 0m)); // how much each one paid - owes
    private object _lock = new();

    public void AddParticipant(IGroupObserver participant)
    {
        if (ParticipantsPaymentDetails.TryAdd(participant, 0m))
            participant.AddedToGroup(this);
    }

    public Expense AddExpense(string description, IGroupObserver payer, decimal amount,
        Dictionary<IGroupObserver, decimal> userShares, SplitStrategyEnum splitType)
    {
        lock (_lock)
        {
            if (!ParticipantsPaymentDetails.ContainsKey(payer))
                throw new ArgumentException("Payer is not part of the group", nameof(payer));

            if (userShares.Keys.Any(user => !ParticipantsPaymentDetails.ContainsKey(user)))
                throw new ArgumentException("Some participants are not part of the group", nameof(userShares));

            var userShareAmounts = SplitStrategyFactory.Instance.GetSplitStrategy(splitType).UserShareAmounts(userShares, amount);
            var expense = new Expense(description, payer, amount, userShareAmounts);

            Expenses.Add(expense);

            ParticipantsPaymentDetails.AddOrUpdate(payer,
                    f => amount,
                    (f, v) => v + amount);
            foreach (var (user, share) in userShareAmounts)
            {
                ParticipantsPaymentDetails.AddOrUpdate(user,
                        f => -amount,
                        (f, v) => v - amount);
            }

            NotifyAllAddedToExpense(userShares.Keys, expense);
            return expense;
        }
    }

    public bool TrySettle(IGroupObserver participant, Expense expense)
    {
        lock (_lock)
        {
            if (!Expenses.Contains(expense))
                return false;
            if (!expense.UserShareAmounts.ContainsKey(participant))
                return false;

            var shareAmount = expense.UserShareAmounts[participant];

            if (!expense.TrySettle(participant, shareAmount))
                return false;

            ParticipantsPaymentDetails[participant] += shareAmount;
            ParticipantsPaymentDetails[expense.Payer] -= shareAmount;

            expense.Payer.GotMoney(this, expense, participant);
            participant.PaidMoney(this, expense);
            return true;
        }
    }

    public void Settle(IGroupObserver participant)
    {
        lock (_lock)
        {
            foreach (var expense in Expenses)
            {
                TrySettle(participant, expense);
            }
        }
    }

    private void NotifyAllAddedToExpense(IEnumerable<IGroupObserver> participants, Expense expense)
    {
        foreach (var participant in participants)
        {
            participant.AddedToExpense(this, expense);
        }
    }

    public override string ToString()
        {
            return Name;
        }
}
