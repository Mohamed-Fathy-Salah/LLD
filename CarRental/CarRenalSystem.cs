using System.Collections.Concurrent;

public class CarRentalSystem
{
    public ConcurrentBag<Car> Cars { get; } = new();
    public ConcurrentBag<Customer> Customers { get; } = new();
    public ConcurrentBag<Request> Requests { get; } = new();
    public ISearch SearchService { get; private set; } = default!;
    public IReserve ReserveService { get; private set; } = default!;

    public void setServices(ISearch searchService, IReserve reserveService)
    {
        SearchService = searchService;
        ReserveService = reserveService;
    }
}
