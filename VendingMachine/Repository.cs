namespace VendingMachine;

public class Repository
{
    private Dictionary<int, Product> _products { get; } = [];

    public bool IsAvailable(int productId, int quantity) =>
        _products.TryGetValue(productId, out Product? product) && product.Quantity >= quantity;

    public int AddNewProduct(string name, int priceInCents)
    {
        var product = new Product(name, priceInCents);
        _products.Add(product.ID, product);
        Console.WriteLine($"Product {product} added!");
        return product.ID;
    }

    public void AddProduct(int productId, int quantity)
    {
        if (!_products.TryGetValue(productId, out Product? product))
        {
            Console.WriteLine($"Product with id:{productId} not found!");
            return;
        }
        product.Quantity += quantity;
    }

    public int GetPriceInCents(int productId, int quantity)
    {
        if (!_products.TryGetValue(productId, out Product? product))
        {
            Console.WriteLine($"Product with id:{productId} not found!");
            return 0;
        }
        return product.PriceInCents * quantity;
    }

    public bool DisposeProduct(int productId, int quantity)
    {
        if (!_products.TryGetValue(productId, out Product? product))
        {
            Console.WriteLine($"Product with id:{productId} not found!");
            return false;
        }
        if (product.Quantity < quantity)
        {
            Console.WriteLine($"there are only {product.Quantity} left!");
            return false;
        }
        product.Quantity -= quantity;
        return true;
    }

    public Product[] GetProducts() => _products.Values.ToArray();
}
