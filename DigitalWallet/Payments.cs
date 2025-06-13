public abstract class Payment(User owner, decimal balance, CurrencyEnum currency)
{
    private static int _nextId = 0;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public User Owner { get; } = owner;
    public decimal Balance { get; private set; } = balance;
    public CurrencyEnum Currency { get; } = currency;

    public void AddBalance(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Amount must be positive");
            return;
        }
        Balance += amount;
    }

    public bool TryTakeBalance(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Amount must be positive");
            return false;
        }
        if (Balance >= amount)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"{this.GetType()} {Id}";
    }
}

public class CreditCardPayment(User owner, decimal balance, CurrencyEnum currency) : Payment(owner, balance, currency)
{
}


public class BankAccountPayment(User owner, decimal balance, CurrencyEnum currency) : Payment(owner, balance, currency)
{
}

