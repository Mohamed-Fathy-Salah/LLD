using System.Collections.Concurrent;

public class SearchService
{
    public Auction[] Search(ConcurrentBag<Auction> auctions, Filter filter)
    {
        return auctions
            .Where(auction =>
                (string.IsNullOrEmpty(filter.name) || auction.Name.Contains(filter.name, StringComparison.OrdinalIgnoreCase)) &&
                (filter.category == null || auction.Category == filter.category) &&
                (filter.minPrice == null || auction.StartingPrice >= filter.minPrice) &&
                (filter.maxPrice == null || auction.StartingPrice <= filter.maxPrice))
            .ToArray();
    }
}
