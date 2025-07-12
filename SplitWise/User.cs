using System.Collections.Concurrent;

namespace SplitWise;

public class User(string name) : IGroupObserver
{
    public string Name { get; set; } = name;
    public ConcurrentDictionary<Group, List<Expense>> GroupExpenses { get; } = [];
    public ConcurrentDictionary<User, (HashSet<Group> groups, decimal owesMe)> UserBalances { get; } = [];

    public Group CreateGroup(string name, User[] participants)
    {
        var group = new Group(name, participants.Append(this).ToHashSet().ToArray());
        if (GroupExpenses.TryAdd(group, []))
            NotifyAllAddedToGroup(participants, group);
        return group;
    }

    private void NotifyAllAddedToGroup(IEnumerable<IGroupObserver> participants, Group group)
    {
        foreach (var participant in participants)
        {
            participant.AddedToGroup(group);
        }
    }


    void IGroupObserver.AddedToExpense(Group group, Expense expense)
    {
        if (GroupExpenses.TryGetValue(group, out var expenses))
        {
            expenses.Add(expense);
            if (this == expense.Payer)
            {
                foreach (var (user, share) in expense.UserShareAmounts)
                {
                    if (user != this) // Skip the payer (this user)
                    {
                        UserBalances.AddOrUpdate((User)user,
                            f => (new HashSet<Group> { group }, share),
                            (f, v) =>
                            {
                                v.groups.Add(group);
                                v.owesMe += share;
                                return v;
                            });
                    }
                }
            }
            else // If this user is not the payer, update balance with the payer
            {
                UserBalances.AddOrUpdate((User)expense.Payer,
                    f => (new HashSet<Group> { group }, -expense.UserShareAmounts[this]),
                    (f, v) =>
                    {
                        v.groups.Add(group);
                        v.owesMe -= expense.UserShareAmounts[this];
                        return v;
                    });
            }
            Console.WriteLine($"{this} added to {expense} in {group}");
        }
    }

    void IGroupObserver.GotMoney(Group group, Expense expense, IGroupObserver participant)
    {
        UserBalances.AddOrUpdate((User)participant,
                f => ([group], -expense.UserShareAmounts[this]),
                (f, v) =>
                {
                    v.groups.Add(group);
                    v.owesMe -= expense.UserShareAmounts[this];
                    return v;
                });
        Console.WriteLine($"{this} got {expense.UserShareAmounts[this]} from {participant}");
    }

    void IGroupObserver.PaidMoney(Group group, Expense expense)
    {
        UserBalances.AddOrUpdate((User)expense.Payer,
                f => ([group], expense.UserShareAmounts[this]),
                (f, v) =>
                {
                    v.groups.Add(group);
                    v.owesMe += expense.UserShareAmounts[this];
                    return v;
                });
        Console.WriteLine($"{this} Paid {expense.UserShareAmounts[this]} to {expense.Payer}");
    }

    void IGroupObserver.AddedToGroup(Group group)
    {
        GroupExpenses.TryAdd(group, []);
        Console.WriteLine($"{this} added to {group}");
    }

    public void SettleBalances(User user)
    {
        lock (UserBalances)
        {
            foreach (var group in UserBalances.GetValueOrDefault(user).groups)
            {
                group.Settle(this);
            }
        }
    }

    public override string ToString()
    {
        return Name;
    }
}

