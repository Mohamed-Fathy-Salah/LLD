public class Request(Guest guest, Room room, DateTime from, DateTime to)
{
    public Guest Guest { get; } = guest;
    public Room Room { get; } = room;
    public DateTime From { get; } = from;
    public DateTime To { get; } = to;
    public bool IsPaid { get; set; } = false;

    public decimal GetRentToBePaid() =>
        Room.RentPerDay * (decimal)(To - From).TotalDays;
}
