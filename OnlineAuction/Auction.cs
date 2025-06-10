using System.Collections.Concurrent;

public class Auction(User owner,
        string name,
        string description,
        CategoryEnum category,
        decimal startingPrice,
        DateTime from,
        DateTime to)
{
    public User Owner { get; } = owner;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public CategoryEnum Category { get; } = category;
    public decimal StartingPrice { get; } = startingPrice;
    public DateTime From { get; } = from;
    public DateTime To { get; } = to;
    public decimal HighestBid { get; private set; } = startingPrice;
    public User? HighestBidder { get; private set; }
    public ConcurrentDictionary<IObserver, byte> Bidders { get; } = [];

    public bool IsActive() => DateTime.Now >= From && DateTime.Now < To;

    private void NotifyAllExcept(IObserver observer)
    {
        foreach (var i in Bidders.Keys.Where(f => f != observer))
        {
            i.Notify(HighestBid);
        }
    }

    public bool AddBid(User user, decimal Amount)
    {
        if (!IsActive())
        {
            Console.WriteLine("Auction is not active.");
            return false;
        }

        if (Amount <= HighestBid)
        {
            Console.WriteLine("Bid must be higher than the current highest bid.");
            return false;
        }

        Bidders.GetOrAdd(user, 0);
        NotifyAllExcept(user);
        HighestBid = Amount;
        HighestBidder = user;
        return true;
    }
}

public enum CategoryEnum
{
    Electronics,
    Fashion,
    HomeAndGarden,
    Sports,
    ToysAndGames,
    Automotive,
    CollectiblesAndArt,
    BooksAndMedia
}
