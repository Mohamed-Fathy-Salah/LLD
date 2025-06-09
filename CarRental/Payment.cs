public interface IPayment
{
    public bool Pay(Customer customer, decimal amount);
}

public class CashPayment : IPayment
{
    public bool Pay(Customer customer, decimal amount)
    {
        Console.WriteLine($"Cash payment of {amount} made by {customer}.");
        return true;
    }
}
