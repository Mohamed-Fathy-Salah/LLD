namespace VendingMachine;

public class Product
{
    private static int _nextId = 1;
    public Product(string name, Step[] steps, double feePercentage, Repository repo)
    {
        ID = Interlocked.Increment(ref _nextId);
        Name = name;
        Steps = steps;
        IngredientsQuantity = steps
            .SelectMany(f => f.IngredientsQuantity)
            .GroupBy(f => f.Key)
            .ToDictionary(g => g.Key, g => g.Sum(f => f.Value));
        PriceInCents = (int)Math.Ceiling(IngredientsQuantity.Sum(f => repo.GetIngredientPriceInCents(f.Key, f.Value)) * feePercentage);
    }
    public int ID { get; init; }
    public string Name { get; init; }
    public Step[] Steps { get; init; }
    public Dictionary<int, int> IngredientsQuantity { get; init; }
    public int PriceInCents { get; init; }

    public override string ToString() => Name;
}
