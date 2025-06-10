public interface IObserver
{
    void Notify(decimal highestBid);
}

public class User(string name, string email, string password) : IObserver
{
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    private string Password = password;

    public bool IsCorrectPassword(string password)
    {
        return Password == password;
    }

    public bool AddBid(Auction auction, decimal bid)
    {
        return auction.AddBid(this, bid);
    }

    public void Notify(decimal highestBid)
    {
        Console.WriteLine($"Notification to {Name}: The highest bid is now {highestBid:C}.");
    }
}
