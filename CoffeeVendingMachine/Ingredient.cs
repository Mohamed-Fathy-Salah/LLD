namespace VendingMachine;

public class Ingredient(string name, int priceInCents)
{
    private static int _nextId = 1;
    public int ID { get; } = Interlocked.Increment(ref _nextId);
    public string Name { get; } = name;
    public int PriceInCents { get; } = priceInCents;
    public int Quantity { get; set; } = 0;

    public override string ToString() => Name;
}
