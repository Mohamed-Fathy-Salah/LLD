public interface ISearchService<T, E>
{
    public List<E> Search(T filter);
}

public record FlightFilter(DateTime? from = null, DateTime? to = null, DestinationsEnum? source = null, DestinationsEnum? destination = null);

public class SearchService(FlightRepository repo) : ISearchService<FlightFilter, Flight>
{
    public List<Flight> Search(FlightFilter filter)
    {
        return repo.GetAllFlights()
            .Where(flight =>
                (!filter.from.HasValue || flight.From >= filter.from.Value) &&
                (!filter.to.HasValue || flight.To <= filter.to.Value) &&
                (!filter.source.HasValue || flight.Source == filter.source.Value) &&
                (!filter.destination.HasValue || flight.Destination == filter.destination.Value))
            .ToList();
    }
}
