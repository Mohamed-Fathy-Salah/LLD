public interface IPayment
{
    public bool Pay(Guest guest, decimal amount);
}

public class CashPayment : IPayment
{
    public bool Pay(Guest guest, decimal amount)
    {
        Console.WriteLine($"Cash payment of {amount} received from {guest}.");
        return true;
    }
}

public class CreditCardPayment : IPayment
{
    public bool Pay(Guest guest, decimal amount)
    {
        Console.WriteLine($"Credit Card payment of {amount} received from {guest}.");
        return true;
    }
}

public class OnlinePayment : IPayment
{
    public bool Pay(Guest guest, decimal amount)
    {
        Console.WriteLine($"Online payment of {amount} received from {guest}.");
        return true;
    }
}
