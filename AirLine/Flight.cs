using System.Collections.Concurrent;

public class FlightRepository
{
    private readonly ConcurrentBag<Flight> _flights = new();

    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
    }

    public List<Flight> GetAllFlights()
    {
        return _flights.ToList();
    }
}

public enum DestinationsEnum
{
    NewYork,
    LosAngeles,
    Chicago,
    Miami,
    Seattle,
    Denver,
    Boston,
    Dallas
}


public class Seat
{
    private static int _nextId = 0;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
}

public class AirCraft
{
    private static int _nextId = 0;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public List<Seat> Seats { get; init; }

    public AirCraft(int capacity)
    {
        Seats = Enumerable.Range(0, capacity)
            .Select(_ => new Seat())
            .ToList();
    }
}

public class Flight(AirCraft airCraft, DateTime from, DateTime to, DestinationsEnum source, DestinationsEnum destination)
{
    public AirCraft AirCraft { get; private set; } = airCraft;
    public DateTime From { get; private set; } = from;
    public DateTime To { get; private set; } = to;
    public DestinationsEnum Source { get; private set; } = source;
    public DestinationsEnum Destination { get; private set; } = destination;
    public decimal PricePerSeat { get; private set; }
    private ConcurrentDictionary<Seat, User> _bookedSeats = new();

    public bool Book(User user, Seat[] seats)
    {
        lock (this)
        {
            Console.WriteLine($"Attempting to book seats for user {user.Name} on flight from {Source} to {Destination}.");
            if (seats.Any(f => _bookedSeats.ContainsKey(f)))
            {
                return false;
            }
            foreach (var seat in seats)
            {
                _bookedSeats[seat] = user;
            }
            return true;
        }
    }

    public int ReturnBookedSeats(User user)
    {
        lock (this)
        {
        int count = 0;
        foreach (var seat in _bookedSeats)
        {
            if (seat.Value == user)
            {
                _bookedSeats.TryRemove(seat.Key, out _);
                count++;
            }
        }
        return count;
        }
    }

    public void NotifyAll()
    {
        foreach (var seat in _bookedSeats.Keys)
        {
            _bookedSeats[seat].FlightChanged(this);
        }
    }

    public List<Seat> GetEmptySeats()
    {
        lock (this)
        {
            return AirCraft.Seats
                .Where(seat => !_bookedSeats.ContainsKey(seat))
                .ToList();
        }
    }

    public void UpdateFlight(AirCraft airCraft, decimal pricePerSeat, DateTime from, DateTime to, DestinationsEnum source, DestinationsEnum destination)
    {
        AirCraft = airCraft;
        From = from;
        To = to;
        Source = source;
        Destination = destination;
        PricePerSeat = pricePerSeat;
        NotifyAll();
    }
}
