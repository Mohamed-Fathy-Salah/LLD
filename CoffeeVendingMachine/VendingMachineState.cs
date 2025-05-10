namespace VendingMachine;

public abstract class VendingMachineState(VendingMachine context) : IVendingMachineState
{
    protected VendingMachine Context { get; set; } = context;
    public abstract string[] AllowedActions { get; }

    public virtual int AddNewIngredient(string name, int priceInCents)
    {
        Console.WriteLine("Not Allowed Operation");
        return -1;
    }

    public virtual void AddIngredient(int ingredientId, int quantity)
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual int AddNewProduct(string name, Step[] steps, double feePercentage)
    {
        Console.WriteLine("Not Allowed Operation");
        return -1;
    }

    public virtual void AddProduct(int productId, int quantity)
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void Cancel()
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void CollectMoney(int amount)
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void Done()
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void InsertMoney(int amount)
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void OpenMachine()
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void RemoveProduct(int productId)
    {
        Console.WriteLine("Not Allowed Operation");
    }

    public virtual void Start()
    {
        Console.WriteLine("Not Allowed Operation");
    }
}
