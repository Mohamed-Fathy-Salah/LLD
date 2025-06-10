using System.Collections.Concurrent;

public class OnlineAuctionSystem(AuthService authService, SearchService searchService)
{
    public ConcurrentBag<Auction> Auctions { get; } = new();
    public AuthService AuthService { get; } = authService;
    public SearchService SearchService { get; } = searchService;
}
