namespace VendingMachine;

public class Product(string name, int priceInCents)
{
    public int ID { get; } = DateTime.UtcNow.Microsecond;
    public string Name { get; } = name;
    public int PriceInCents { get; } = priceInCents;
    public int Quantity { get; set; } = 0;

    public override string ToString() => Name;
}
