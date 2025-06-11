using System.Collections.Concurrent;

public sealed class HotelManagementSystem
{
    private static Lazy<HotelManagementSystem> _instance = new(() => new HotelManagementSystem());
    public ConcurrentBag<Guest> _guests { get; } = new();
    public ConcurrentBag<Room> _rooms { get; } = new();
    private ConcurrentDictionary<Room, List<Request>> _requests { get; } = new();

    private HotelManagementSystem() { }

    public static HotelManagementSystem Instance => _instance.Value;

    public Request? BookRoom(Guest guest, Room room, DateTime from, DateTime to)
    {
        if (_requests.GetValueOrDefault(room)?.Any(f => (f.From < from && f.To > from) || (f.From < to && f.To > to)) ?? false)
        {
            var request = new Request(guest, room, from, to);
            if (!_requests.TryAdd(room, new List<Request> { request }))
            {
                _requests[room].Add(request);
            }
            room.AssignGuest(guest);
            guest.AssignRoom(room);
            return request;
        }
        return null;
    }

    public bool FreeRoom(Room room)
    {
        room.AssignedGuest?.FreeRoom();
        room.FreeRoom();
        return true;
    }

    public bool HandlePayment(Request request, IPayment payment)
    {
        lock (request)
        {
            if (request.IsPaid)
                return false;

            if (payment.Pay(request.Guest, request.GetRentToBePaid()))
            {
                request.IsPaid = true;
                return true;
            }
            return false;
        }
    }
}
