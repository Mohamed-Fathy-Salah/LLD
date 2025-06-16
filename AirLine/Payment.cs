public interface IPayment
{
    public bool Pay(User user, decimal amount);
}

public class CashPayment : IPayment
{
    public bool Pay(User user, decimal amount)
    {
        Console.WriteLine($"User {user.Name} paid {amount} using cash.");
        return true;
    }
}

public class CreditCardPayment : IPayment
{
    public bool Pay(User user, decimal amount)
    {
        Console.WriteLine($"User {user.Name} paid {amount} using credit card.");
        return true;
    }
}
