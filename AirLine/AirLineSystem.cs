using System.Collections.Concurrent;

public class AirLineSystem
{
    private AirLineSystem(ISearchService<FlightFilter, Flight> searchService, FlightRepository flightRepository)
    {
        SearchService = searchService;
        FlightRepository = flightRepository;
    }

    private static Lazy<AirLineSystem>? _instance;
    public static AirLineSystem Instance => _instance?.Value ?? throw new InvalidOperationException("AirLineSystem is not initialized.");
    public ISearchService<FlightFilter, Flight> SearchService { get; private set; }
    public FlightRepository FlightRepository { get; private set; }
    public ConcurrentBag<User> Users { get; } = [];

    public static void Initialize(ISearchService<FlightFilter, Flight> searchService, FlightRepository flightRepository)
    {
        if (_instance != null)
            throw new InvalidOperationException("AirLineSystem is already initialized.");

        _instance = new Lazy<AirLineSystem>(() => new AirLineSystem(searchService, flightRepository));
    }

    public bool BookFlight(User user, Flight flight, Seat[] seats, IPayment payment)
    {
        if (seats.Length == 0)
        {
            Console.WriteLine("Booking failed: No seats selected.");
            return false;
        }
        if (flight.Book(user, seats))
        {
            if (payment.Pay(user, flight.PricePerSeat * seats.Length))
            {
                Console.WriteLine($"Booking successful for user {user.Name} on flight from {flight.Source} to {flight.Destination}.");
                return true;
            }
            else
            {
                flight.ReturnBookedSeats(user);
                Console.WriteLine("Payment failed. Booking cancelled.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Booking failed: Seats already booked.");
            return false;
        }
    }

    public bool CancelFlight(User user, Flight flight, IPayment payment)
    {
        lock (flight)
        {
            if (payment.Pay(user, flight.PricePerSeat * flight.ReturnBookedSeats(user)))
            {
                Console.WriteLine($"Booking cancelled for user {user.Name} on flight from {flight.Source} to {flight.Destination}.");
                return true;
            }
        }
        Console.WriteLine($"Refund failed for user {user.Name}. Please contact support.");
        return false;
    }

    public void UpdateFlight(Flight flight, AirCraft airCraft, DateTime from, DateTime to, DestinationsEnum source, DestinationsEnum destination, decimal pricePerSeat)
    {
        flight.UpdateFlight(airCraft, pricePerSeat, from, to, source, destination);
    }
}
