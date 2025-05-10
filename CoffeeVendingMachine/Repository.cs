namespace VendingMachine;

public class Repository
{
    private Dictionary<int, Product> _products { get; } = [];
    private Dictionary<int, Ingredient> _ingredients { get; } = [];

    public bool IsAvailable(int ingredientId, int quantity) =>
        _ingredients.TryGetValue(ingredientId, out Ingredient? ingredient) && ingredient.Quantity >= quantity;

    public int AddNewIngredient(string name, int priceInCents)
    {
        var ingredient = new Ingredient(name, priceInCents);
        _ingredients.Add(ingredient.ID, ingredient);
        Console.WriteLine($"Ingredient {ingredient} added!");
        return ingredient.ID;
    }

    public int AddNewProduct(string name, Step[] steps, double feePercentage)
    {
        var product = new Product(name, steps, feePercentage, this);
        _products.Add(product.ID, product);
        Console.WriteLine($"Product {product} added!");
        return product.ID;
    }

    public void AddIngredient(int ingredientId, int quantity)
    {
        if (!_ingredients.TryGetValue(ingredientId, out Ingredient? ingredient))
        {
            Console.WriteLine($"Ingredient with id:{ingredientId} not found!");
            return;
        }
        ingredient.Quantity += quantity;
    }

    public int GetIngredientPriceInCents(int ingredientId, int quantity)
    {
        if (!_ingredients.TryGetValue(ingredientId, out Ingredient? ingredient))
        {
            Console.WriteLine($"Ingredient with id:{ingredientId} not found!");
            return 0;
        }
        return ingredient.PriceInCents * quantity;
    }

    public int GetProductPriceInCents(int id, int quantity)
    {
        if (!_products.TryGetValue(id, out Product? product))
        {
            Console.WriteLine($"product with id:{id} not found!");
            return 0;
        }
        return product.PriceInCents * quantity;
    }

    public bool DisposeIngredient(int ingredientId, int quantity)
    {
        if (!_ingredients.TryGetValue(ingredientId, out Ingredient? ingredient))
        {
            Console.WriteLine($"Ingredient with id:{ingredientId} not found!");
            return false;
        }
        if (ingredient.Quantity < quantity)
        {
            Console.WriteLine($"there are only {ingredient.Quantity} left!");
            return false;
        }
        ingredient.Quantity -= quantity;
        return true;
    }

    public bool DisposeProductIngredients(int productId, int quantity)
    {
        if (!_products.TryGetValue(productId, out Product? product))
        {
            Console.WriteLine($"product not found!");
            return false;
        }
        List<KeyValuePair<int, int>> temp = [];
        foreach (var id_q in product.IngredientsQuantity)
        {
            if (!_ingredients.TryGetValue(id_q.Key, out Ingredient? ingredient))
            {
                Console.WriteLine($"Ingredient not found!");
                Rollback(temp);
                return false;
            }
            var ingredientQuantity = id_q.Value * quantity;
            if (ingredient.Quantity < ingredientQuantity)
            {
                Console.WriteLine("not enough ingredients");
                Rollback(temp);
                return false;
            }
            temp.Add(new(id_q.Key, ingredientQuantity));
            ingredient.Quantity -= ingredientQuantity;
        }
        return true;
    }

    public void ReturnProductIngredients(int productId, int quantity)
    {
        if (!_products.TryGetValue(productId, out Product? product))
        {
            Console.WriteLine($"product not found!");
            return;
        }
        foreach (var id_q in product.IngredientsQuantity)
            _ingredients[id_q.Key].Quantity += id_q.Value * quantity;
    }

    private void Rollback(List<KeyValuePair<int, int>> ingredientsQuantity)
    {
        foreach (var id_q in ingredientsQuantity)
            _ingredients[id_q.Key].Quantity += id_q.Value;
    }

    public Ingredient[] GetIngredients() => _ingredients.Values.ToArray();

    public Product[] GetProducts() => _products.Values.ToArray();

    public Product? GetProduct(int productId) => _products.GetValueOrDefault(productId);
}
