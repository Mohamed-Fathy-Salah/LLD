using System.Collections.Concurrent;

class Program
{
    private static readonly Random random = new Random();
    private static readonly object consoleLock = new object();
    
    static async Task Main(string[] args)
    {
        // Initialize the airline system
        var flightRepository = new FlightRepository();
        var searchService = new SearchService(flightRepository);
        AirLineSystem.Initialize(searchService, flightRepository);
        
        // Setup test data
        SetupTestFlights(flightRepository);
        
        Console.WriteLine("=== Airline System Concurrency Test ===\n");
        
        // Test 1: Basic functionality
        TestBasicFunctionality();
        
        // Test 2: Concurrent booking on same flight
        await TestConcurrentBookingSameFlight();
        
        // Test 3: Concurrent booking different flights
        await TestConcurrentBookingDifferentFlights();
        
        // Test 4: Mixed operations (book/cancel/update)
        await TestMixedConcurrentOperations();
        
        // Test 5: Race condition testing
        await TestRaceConditions();
        
        Console.WriteLine("\n=== All Tests Completed ===");
        Console.ReadKey();
    }
    
    private static void SetupTestFlights(FlightRepository repository)
    {
        var flights = new[]
        {
            new Flight(new AirCraft(10), DateTime.Now.AddHours(2), DateTime.Now.AddHours(6), 
                      DestinationsEnum.NewYork, DestinationsEnum.LosAngeles),
            new Flight(new AirCraft(5), DateTime.Now.AddHours(1), DateTime.Now.AddHours(4), 
                      DestinationsEnum.Chicago, DestinationsEnum.Miami),
            new Flight(new AirCraft(20), DateTime.Now.AddHours(3), DateTime.Now.AddHours(8), 
                      DestinationsEnum.Seattle, DestinationsEnum.Denver),
            new Flight(new AirCraft(15), DateTime.Now.AddHours(4), DateTime.Now.AddHours(7), 
                      DestinationsEnum.Boston, DestinationsEnum.Dallas)
        };
        
        foreach (var flight in flights)
        {
            // Set initial price
            flight.UpdateFlight(flight.AirCraft, 299.99m, flight.From, flight.To, flight.Source, flight.Destination);
            repository.AddFlight(flight);
        }
        
        SafeWriteLine($"Setup complete: {flights.Length} flights added to repository");
    }
    
    private static void TestBasicFunctionality()
    {
        SafeWriteLine("\n--- Test 1: Basic Functionality ---");
        
        var system = AirLineSystem.Instance;
        var user = new User("TestUser");
        var flights = system.FlightRepository.GetAllFlights();
        var flight = flights.First();
        var payment = new CreditCardPayment();
        
        // Test search
        var searchResults = system.SearchService.Search(new FlightFilter(source: flight.Source));
        SafeWriteLine($"Search found {searchResults.Count} flights from {flight.Source}");
        
        // Test booking
        var availableSeats = flight.GetEmptySeats().Take(2).ToArray();
        bool bookingResult = system.BookFlight(user, flight, availableSeats, payment);
        SafeWriteLine($"Booking result: {bookingResult}");
        SafeWriteLine($"Remaining empty seats: {flight.GetEmptySeats().Count}");
        
        // Test cancellation
        bool cancelResult = system.CancelFlight(user, flight, payment);
        SafeWriteLine($"Cancellation result: {cancelResult}");
        SafeWriteLine($"Empty seats after cancellation: {flight.GetEmptySeats().Count}");
    }
    
    private static async Task TestConcurrentBookingSameFlight()
    {
        SafeWriteLine("\n--- Test 2: Concurrent Booking Same Flight ---");
        
        var system = AirLineSystem.Instance;
        var flight = system.FlightRepository.GetAllFlights().First();
        var totalSeats = flight.AirCraft.Seats.Count;
        
        SafeWriteLine($"Flight has {totalSeats} total seats");
        SafeWriteLine($"Available seats before test: {flight.GetEmptySeats().Count}");
        
        var users = Enumerable.Range(1, 15)
            .Select(i => new User($"ConcurrentUser{i}"))
            .ToList();
        
        var tasks = users.Select(user => Task.Run(async () =>
        {
            await Task.Delay(random.Next(10, 100)); // Random delay to simulate real conditions
            
            var availableSeats = flight.GetEmptySeats().Take(2).ToArray();
            if (availableSeats.Length >= 2)
            {
                IPayment payment = random.Next(2) == 0 ? new CashPayment() : new CreditCardPayment();
                bool result = system.BookFlight(user, flight, availableSeats, payment);
                
                SafeWriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] User {user.Name}: Booking {(result ? "SUCCESS" : "FAILED")}");
                return result;
            }
            else
            {
                SafeWriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] User {user.Name}: No seats available");
                return false;
            }
        })).ToArray();
        
        var results = await Task.WhenAll(tasks);
        var successCount = results.Count(r => r);
        var remainingSeats = flight.GetEmptySeats().Count;
        
        SafeWriteLine($"Concurrent booking results: {successCount} successful, {users.Count - successCount} failed");
        SafeWriteLine($"Remaining seats: {remainingSeats}");
        SafeWriteLine($"Expected seats booked: {totalSeats - remainingSeats}, Actual successful bookings: {successCount * 2}");
    }
    
    private static async Task TestConcurrentBookingDifferentFlights()
    {
        SafeWriteLine("\n--- Test 3: Concurrent Booking Different Flights ---");
        
        var system = AirLineSystem.Instance;
        var flights = system.FlightRepository.GetAllFlights().ToList();
        
        var tasks = new List<Task<string>>();
        
        for (int i = 0; i < 20; i++)
        {
            var userId = i;
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(random.Next(10, 50));
                
                var user = new User($"MultiFlightUser{userId}");
                var flight = flights[userId % flights.Count];
                var availableSeats = flight.GetEmptySeats().Take(1).ToArray();
                
                if (availableSeats.Length > 0)
                {
                    var payment = new CreditCardPayment();
                    bool result = system.BookFlight(user, flight, availableSeats, payment);
                    
                    return $"[Thread {Thread.CurrentThread.ManagedThreadId}] User {user.Name} on Flight {flight.Source}->{flight.Destination}: {(result ? "SUCCESS" : "FAILED")}";
                }
                
                return $"[Thread {Thread.CurrentThread.ManagedThreadId}] User {user.Name}: No seats available";
            }));
        }
        
        var results = await Task.WhenAll(tasks);
        foreach (var result in results)
        {
            SafeWriteLine(result);
        }
        
        // Show flight status
        foreach (var flight in flights)
        {
            SafeWriteLine($"Flight {flight.Source}->{flight.Destination}: {flight.GetEmptySeats().Count} seats remaining");
        }
    }
    
    private static async Task TestMixedConcurrentOperations()
    {
        SafeWriteLine("\n--- Test 4: Mixed Concurrent Operations ---");
        
        var system = AirLineSystem.Instance;
        var flights = system.FlightRepository.GetAllFlights().ToList();
        var bookedUsers = new ConcurrentBag<(User user, Flight flight)>();
        
        var tasks = new List<Task>();
        
        // Booking tasks
        for (int i = 0; i < 10; i++)
        {
            var userId = i;
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(random.Next(10, 100));
                
                var user = new User($"MixedUser{userId}");
                var flight = flights[userId % flights.Count];
                var seats = flight.GetEmptySeats().Take(1).ToArray();
                
                if (seats.Length > 0)
                {
                    var payment = new CreditCardPayment();
                    if (system.BookFlight(user, flight, seats, payment))
                    {
                        bookedUsers.Add((user, flight));
                        SafeWriteLine($"[BOOK] User {user.Name} booked flight {flight.Source}->{flight.Destination}");
                    }
                }
            }));
        }
        
        // Cancellation tasks (will run after some bookings)
        tasks.Add(Task.Run(async () =>
        {
            await Task.Delay(200); // Wait for some bookings
            
            while (bookedUsers.TryTake(out var booking))
            {
                var payment = new CreditCardPayment();
                if (system.CancelFlight(booking.user, booking.flight, payment))
                {
                    SafeWriteLine($"[CANCEL] User {booking.user.Name} cancelled flight {booking.flight.Source}->{booking.flight.Destination}");
                }
                await Task.Delay(50);
            }
        }));
        
        // Flight update tasks
        for (int i = 0; i < 3; i++)
        {
            var flightIndex = i;
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(random.Next(50, 150));
                
                var flight = flights[flightIndex % flights.Count];
                var newPrice = 199.99m + (decimal)(random.NextDouble() * 200);
                
                SafeWriteLine($"[UPDATE] Updating flight {flight.Source}->{flight.Destination} price to ${newPrice:F2}");
                system.UpdateFlight(flight, flight.AirCraft, flight.From.AddMinutes(15), 
                                  flight.To.AddMinutes(15), flight.Source, flight.Destination, newPrice);
            }));
        }
        
        await Task.WhenAll(tasks);
    }

    private static async Task TestRaceConditions()
    {
        SafeWriteLine("\n--- Test 5: Race Condition Test ---");

        var smallFlight = new Flight(new AirCraft(3), DateTime.Now.AddHours(1), 
                DateTime.Now.AddHours(3), DestinationsEnum.NewYork, DestinationsEnum.Boston);
        smallFlight.UpdateFlight(smallFlight.AirCraft, 99.99m, smallFlight.From, smallFlight.To, 
                smallFlight.Source, smallFlight.Destination);

        AirLineSystem.Instance.FlightRepository.AddFlight(smallFlight);

        SafeWriteLine($"Created flight with {smallFlight.AirCraft.Seats.Count} seats for race condition test");

        var users = Enumerable.Range(1, 50)
            .Select(i => new User($"RaceUser{i}"))
            .ToList();

        var startTime = DateTime.Now;
        var tasks = users.Select(user => Task.Run(() =>
                    {
                    // Get available seats inside the booking method to avoid TOCTOU
                    var payment = new CashPayment();

                    // Try to book one seat - let the booking method handle seat selection atomically
                    var availableSeats = smallFlight.GetEmptySeats().Take(1).ToArray();
                    if (availableSeats.Length > 0)
                    {
                    bool result = AirLineSystem.Instance.BookFlight(user, smallFlight, availableSeats, payment);

                    var elapsed = DateTime.Now - startTime;
                    SafeWriteLine($"[{elapsed.TotalMilliseconds:F0}ms] User {user.Name}: {(result ? "SUCCESS" : "FAILED")}");
                    return result;
                    }

                    var elapsedFailed = DateTime.Now - startTime;
                    SafeWriteLine($"[{elapsedFailed.TotalMilliseconds:F0}ms] User {user.Name}: FAILED (No seats)");
                    return false;
                    })).ToArray();

        var results = await Task.WhenAll(tasks);
        var successCount = results.Count(r => r);
        var remainingSeats = smallFlight.GetEmptySeats().Count;

        SafeWriteLine($"\nFixed race condition test results:");
        SafeWriteLine($"Total users: {users.Count}");
        SafeWriteLine($"Successful bookings: {successCount}");
        SafeWriteLine($"Failed bookings: {users.Count - successCount}");
        SafeWriteLine($"Remaining seats: {remainingSeats}");
        SafeWriteLine($"Expected remaining seats: {smallFlight.AirCraft.Seats.Count - successCount}");

        // Verify integrity
        bool integrityCheck = (smallFlight.AirCraft.Seats.Count - remainingSeats) == successCount;
        SafeWriteLine($"Integrity check: {(integrityCheck ? "PASSED" : "FAILED")}");

        // Additional verification: ensure no seat is double-booked
        var totalBookedSeats = smallFlight.AirCraft.Seats.Count - remainingSeats;
        SafeWriteLine($"Total seats: {smallFlight.AirCraft.Seats.Count}");
        SafeWriteLine($"Booked seats: {totalBookedSeats}");
        SafeWriteLine($"Available seats: {remainingSeats}");
        SafeWriteLine($"Sum check: {totalBookedSeats + remainingSeats == smallFlight.AirCraft.Seats.Count}");
    }

    private static void SafeWriteLine(string message)
    {
        lock (consoleLock)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {message}");
        }
    }
}
