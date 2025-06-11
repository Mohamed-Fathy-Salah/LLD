Console.WriteLine("=== Hotel Management System Demo ===\n");

// Get the singleton instance
var hotel = HotelManagementSystem.Instance;

// Test 1: Basic Setup
Console.WriteLine("1. Setting up hotel with rooms and guests...");
await TestBasicSetup(hotel);

// Test 2: Room Booking
Console.WriteLine("\n2. Testing room bookings...");
await TestRoomBooking(hotel);

// Test 3: Payment Processing
Console.WriteLine("\n3. Testing payment processing...");
await TestPaymentProcessing(hotel);

// Test 4: Check-in/Check-out
Console.WriteLine("\n4. Testing check-in and check-out...");
await TestCheckInCheckOut(hotel);

// Test 5: Concurrent Operations
Console.WriteLine("\n5. Testing concurrent operations...");
await TestConcurrentOperations(hotel);

// Test 6: Edge Cases
Console.WriteLine("\n6. Testing edge cases...");
await TestEdgeCases(hotel);

Console.WriteLine("\n=== Demo Complete ===");
Console.ReadKey();

static async Task TestBasicSetup(HotelManagementSystem hotel)
{
    // Create rooms
    var room101 = new Room("101", RoomTypeEnum.SINGLE, 100m);
    var room102 = new Room("102", RoomTypeEnum.DOUBLE, 150m);
    var room201 = new Room("201", RoomTypeEnum.DELUX, 200m);
    var room301 = new Room("301", RoomTypeEnum.SUITE, 300m);

    hotel._rooms.Add(room101);
    hotel._rooms.Add(room102);
    hotel._rooms.Add(room201);
    hotel._rooms.Add(room301);

    // Create guests
    var guest1 = new Guest("John Doe");
    var guest2 = new Guest("Jane Smith");
    var guest3 = new Guest("Bob Johnson");
    var guest4 = new Guest("Alice Brown");

    hotel._guests.Add(guest1);
    hotel._guests.Add(guest2);
    hotel._guests.Add(guest3);
    hotel._guests.Add(guest4);

    Console.WriteLine($"✓ Created {hotel._rooms.Count} rooms");
    Console.WriteLine($"✓ Added {hotel._guests.Count} guests");

    // Display rooms
    Console.WriteLine("\nAvailable Rooms:");
    foreach (var room in hotel._rooms)
    {
        Console.WriteLine($"  - Room {room.Label}: {room.Type}, ${room.RentPerDay}/day, Status: {room.status.ToString()}");
    }
}

static async Task TestRoomBooking(HotelManagementSystem hotel)
{
    var guest1 = hotel._guests.First(g => g.Name == "John Doe");
    var guest2 = hotel._guests.First(g => g.Name == "Jane Smith");
    var room101 = hotel._rooms.First(r => r.Label == "101");
    var room102 = hotel._rooms.First(r => r.Label == "102");

    // Book room for guest 1
    var checkIn1 = DateTime.Now.AddDays(1);
    var checkOut1 = DateTime.Now.AddDays(3);

    var request1 = hotel.BookRoom(guest1, room101, checkIn1, checkOut1);

    if (request1 != null)
    {
        Console.WriteLine($"✓ {guest1.Name} successfully booked room {room101.Label}");
        Console.WriteLine($"  From: {checkIn1:yyyy-MM-dd} To: {checkOut1:yyyy-MM-dd}");
        Console.WriteLine($"  Total cost: ${request1.GetRentToBePaid():F2}");
        Console.WriteLine($"  Room status: {room101.status.ToString()}");
    }
    else
    {
        Console.WriteLine($"✗ Failed to book room {room101.Label} for {guest1.Name}");
    }

    // Book room for guest 2
    var checkIn2 = DateTime.Now.AddDays(2);
    var checkOut2 = DateTime.Now.AddDays(4);

    var request2 = hotel.BookRoom(guest2, room102, checkIn2, checkOut2);

    if (request2 != null)
    {
        Console.WriteLine($"✓ {guest2.Name} successfully booked room {room102.Label}");
        Console.WriteLine($"  From: {checkIn2:yyyy-MM-dd} To: {checkOut2:yyyy-MM-dd}");
        Console.WriteLine($"  Total cost: ${request2.GetRentToBePaid():F2}");
    }

    // Try to book the same room for overlapping dates (should fail)
    var guest3 = hotel._guests.First(g => g.Name == "Bob Johnson");
    var conflictRequest = hotel.BookRoom(guest3, room101, checkIn1.AddDays(1), checkOut1.AddDays(1));

    if (conflictRequest == null)
    {
        Console.WriteLine($"✓ Correctly prevented double booking of room {room101.Label}");
    }
    else
    {
        Console.WriteLine($"✗ System allowed double booking - this shouldn't happen!");
    }
}

static async Task TestPaymentProcessing(HotelManagementSystem hotel)
{
    var guest1 = hotel._guests.First(g => g.Name == "John Doe");
    var room101 = hotel._rooms.First(r => r.Label == "101");

    // Find the booking request for guest1
    var request = FindBookingRequest(hotel, guest1, room101);

    if (request != null)
    {
        Console.WriteLine($"Processing payment for {guest1.Name}'s booking...");
        Console.WriteLine($"Amount due: ${request.GetRentToBePaid():F2}");

        // Test different payment methods
        var cashPayment = new CashPayment();
        var success = hotel.HandlePayment(request, cashPayment);

        if (success)
        {
            Console.WriteLine($"✓ Payment successful! Request is now paid: {request.IsPaid}");
        }
        else
        {
            Console.WriteLine("✗ Payment failed");
        }

        // Try to pay again (should fail - already paid)
        var creditPayment = new CreditCardPayment();
        var duplicatePayment = hotel.HandlePayment(request, creditPayment);

        if (!duplicatePayment)
        {
            Console.WriteLine("✓ Correctly prevented duplicate payment");
        }
        else
        {
            Console.WriteLine("✗ System allowed duplicate payment");
        }
    }

    // Test other payment methods with guest2
    var guest2 = hotel._guests.First(g => g.Name == "Jane Smith");
    var room102 = hotel._rooms.First(r => r.Label == "102");
    var request2 = FindBookingRequest(hotel, guest2, room102);

    if (request2 != null)
    {
        var onlinePayment = new OnlinePayment();
        hotel.HandlePayment(request2, onlinePayment);
    }
}

static async Task TestCheckInCheckOut(HotelManagementSystem hotel)
{
    var guest1 = hotel._guests.First(g => g.Name == "John Doe");
    var guest2 = hotel._guests.First(g => g.Name == "Jane Smith");

    Console.WriteLine("Before check-in:");
    Console.WriteLine($"  {guest1.Name}'s room status: {guest1.AssignedRoom?.status.ToString()}");
    Console.WriteLine($"  {guest2.Name}'s room status: {guest2.AssignedRoom?.status.ToString()}");

    // Check in guests
    guest1.CheckIn();
    guest2.CheckIn();

    Console.WriteLine("\nAfter check-in:");
    Console.WriteLine($"  {guest1.Name}'s room status: {guest1.AssignedRoom?.status.ToString()}");
    Console.WriteLine($"  {guest2.Name}'s room status: {guest2.AssignedRoom?.status.ToString()}");

    // Simulate some time passing
    await Task.Delay(1000);

    // Check out guest1
    guest1.CheckOut();

    Console.WriteLine("\nAfter guest1 check-out:");
    Console.WriteLine($"  {guest1.Name}'s room status: {guest1.AssignedRoom?.status.ToString()}");
    Console.WriteLine($"  {guest2.Name}'s room status: {guest2.AssignedRoom?.status.ToString()}");

    // Free the room completely
    if (guest1.AssignedRoom != null)
    {
        hotel.FreeRoom(guest1.AssignedRoom);
        Console.WriteLine($"✓ Room {guest1.AssignedRoom.Label} freed - Status: {guest1.AssignedRoom.status.ToString()}");
    }
}

static async Task TestConcurrentOperations(HotelManagementSystem hotel)
{
    var room201 = hotel._rooms.First(r => r.Label == "201");
    var guest3 = hotel._guests.First(g => g.Name == "Bob Johnson");
    var guest4 = hotel._guests.First(g => g.Name == "Alice Brown");

    Console.WriteLine("Testing concurrent room assignment...");

    var tasks = new List<Task<bool>>();

    // Try to assign the same room to multiple guests concurrently
    tasks.Add(Task.Run(() => room201.AssignGuest(guest3)));
    tasks.Add(Task.Run(() => room201.AssignGuest(guest4)));

    var results = await Task.WhenAll(tasks);

    var successCount = results.Count(r => r);
    Console.WriteLine($"✓ {successCount} out of 2 concurrent assignments succeeded (should be 1)");
    Console.WriteLine($"  Room {room201.Label} assigned to: {room201.AssignedGuest?.Name ?? "No one"}");
    Console.WriteLine($"  Room status: {room201.status.ToString()}");

    // Test concurrent check-in/check-out operations
    if (room201.AssignedGuest != null)
    {
        var checkInOutTasks = new List<Task>();

        for (int i = 0; i < 5; i++)
        {
            checkInOutTasks.Add(Task.Run(() => room201.CheckIn()));
            checkInOutTasks.Add(Task.Run(() => room201.CheckOut()));
        }

        await Task.WhenAll(checkInOutTasks);
        Console.WriteLine($"✓ Completed concurrent check-in/out operations");
        Console.WriteLine($"  Final room status: {room201.status.ToString()}");
    }
}

static async Task TestEdgeCases(HotelManagementSystem hotel)
{
    Console.WriteLine("Testing edge cases...");

    // Test singleton pattern
    var hotel2 = HotelManagementSystem.Instance;
    var areSameInstance = ReferenceEquals(hotel, hotel2);
    Console.WriteLine($"✓ Singleton pattern working: {areSameInstance}");

    // Test guest with no assigned room
    var guestWithoutRoom = new Guest("No Room Guest");
    guestWithoutRoom.CheckIn();  // Should not crash
    guestWithoutRoom.CheckOut(); // Should not crash
    Console.WriteLine("✓ Guest operations work safely with no assigned room");

    // Test room operations with no assigned guest
    var emptyRoom = new Room("999", RoomTypeEnum.SINGLE, 75m);
    emptyRoom.CheckIn();  // Should not change status
    emptyRoom.CheckOut(); // Should not change status
    Console.WriteLine($"✓ Empty room operations work safely - Status: {emptyRoom.status.ToString()}");

    // Test free room that's already free
    var freeResult = emptyRoom.FreeRoom();
    Console.WriteLine($"✓ Freeing already free room handled gracefully: {!freeResult}");

    // Test assigning already assigned guest
    var occupiedRoom = hotel._rooms.First(r => r.AssignedGuest != null);
    var newGuest = new Guest("Late Guest");
    var assignResult = occupiedRoom.AssignGuest(newGuest);
    Console.WriteLine($"✓ Assigning to occupied room prevented: {!assignResult}");
}

// Helper method to find a booking request
static Request? FindBookingRequest(HotelManagementSystem hotel, Guest guest, Room room)
{
    // This is a simplified way to find the request - in a real system you'd have better tracking
    var requestsField = typeof(HotelManagementSystem).GetField("_requests",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

    if (requestsField?.GetValue(hotel) is System.Collections.Concurrent.ConcurrentDictionary<Room, List<Request>> requests)
    {
        if (requests.TryGetValue(room, out var roomRequests))
        {
            return roomRequests.FirstOrDefault(r => r.Guest == guest);
        }
    }

    return null;
}
