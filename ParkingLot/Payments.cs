public interface IPayment {
    public void ProcessPayment(Ticket t);
}

public class CashPayment : IPayment
{
    public void ProcessPayment(Ticket t)
    {
        var dueAmount = (DateTime.UtcNow - t.Time).TotalMilliseconds * 5;
        Console.WriteLine($"cash payment for {t.Spot.Vehicle} payed {dueAmount}.");
    }
}
