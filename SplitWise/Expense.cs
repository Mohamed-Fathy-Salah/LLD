using System.Collections.Concurrent;

namespace SplitWise;

public class Expense(string description, IGroupObserver payer, decimal amount, Dictionary<IGroupObserver, decimal> userShareAmounts)
{
    public string Description { get; set; } = description;
    public IGroupObserver Payer { get; set; } = payer;
    public decimal Amount { get; set; } = amount;
    public Dictionary<IGroupObserver, decimal> UserShareAmounts { get; } = userShareAmounts;
    public ConcurrentDictionary<IGroupObserver, decimal> UserPayments { get; } = new(userShareAmounts.Keys.ToDictionary(f => f, f => f == payer ? amount : 0m));
    public DateTime CreatedAt { get; } = DateTime.Now;

    public bool TrySettle(IGroupObserver settler, decimal amount)
    {
        if (!UserShareAmounts.ContainsKey(settler))
            return false;

        decimal currentPayment = UserPayments[settler];
        decimal newPayment = currentPayment + amount;
        decimal share = UserShareAmounts[settler];

        if (newPayment > share)
            return false;

        UserPayments.AddOrUpdate(settler, newPayment, (key, oldValue) => newPayment);
        UserPayments.AddOrUpdate(Payer, UserPayments[Payer] - amount, (key, oldValue) => oldValue - amount);
        return true;
    }

    public override string ToString()
        {
            return Description;
        }
}

