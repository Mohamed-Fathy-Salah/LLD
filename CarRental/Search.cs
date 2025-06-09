public interface ISearch
{
    public List<Car> Search(Filter filter);
}

public class SearchService(CarRentalSystem repo) : ISearch
{
    public List<Car> Search(Filter filter)
    {
        return repo.Cars.Where(car =>
            (!filter.type.HasValue || car.Type == filter.type) &&
            (!filter.minPrice.HasValue || car.RentPerDay >= filter.minPrice) &&
            (!filter.maxPrice.HasValue || car.RentPerDay <= filter.maxPrice) &&
            (filter.isAvailable == null || car.IsAvailable() == filter.isAvailable))
            .ToList();
    }
}
