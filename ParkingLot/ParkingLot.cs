using System.Collections.Concurrent;

public class ParkingLot(List<Level> levels)
{
    public List<Level> Levels { get; } = levels;

    public ConcurrentDictionary<string, Ticket> ActiveTickets { get; private set; } = [];

    public Ticket? ParkVehicle(Vehicle v, SpotAllocationStrategy strategy)
    {
        foreach (var level in Levels)
        {
            var spot = strategy.Allocate(level, v);
            if (spot is not null)
            {
                var ticket = new Ticket(level, spot);
                ActiveTickets.TryAdd(v.PlateNumber, ticket);
                Console.WriteLine(ticket);
                return ticket;
            }
        }
        Console.WriteLine("no empty spots :(");
        return null;
    }

    public void ExitVehicle(Vehicle v, IPayment payment)
    {
        ActiveTickets.Remove(v.PlateNumber, out var ticket);
        if (ticket is null) {
            Console.WriteLine("not found :(");
            return;
        }
        payment.ProcessPayment(ticket);
        ticket.Level.FreeSpot(ticket.Spot);
    }
}
