using VendingMachine.States;

namespace VendingMachine;

public class VendingMachine : IVendingMachineState
{
    public int AmountInCents { get; set; }
    public Repository Repository { get; init; }
    public VendingMachineState CurrentState { get; set; }
    public Dictionary<int, int> ProductQuantityInCart { get; }
    public int PaidAmountInCents { get; set; }
    public int AmountToBePaidInCents { get; set; }

    public VendingMachine(int amountInCents)
    {
        AmountInCents = amountInCents;
        Repository = new Repository();
        CurrentState = StateFactory.Get<IdleState>(this);
        ProductQuantityInCart = [];
        PaidAmountInCents = 0;
        AmountToBePaidInCents = 0;
    }

    public Product[] GetProducts() => Repository.GetProducts();

    public int AddNewProduct(string name, int priceInCents) =>
        CurrentState.AddNewProduct(name, priceInCents);

    public void AddProduct(int productId, int quantity) =>
        CurrentState.AddProduct(productId, quantity);

    public void Cancel() =>
        CurrentState.Cancel();

    public void CollectMoney(int amount) =>
        CurrentState.CollectMoney(amount);

    public void Done() => 
        CurrentState.Done();

    public void InsertMoney(int amount) =>
        CurrentState.InsertMoney(amount);

    public void OpenMachine() =>
        CurrentState.OpenMachine();

    public void RemoveProduct(int productId) =>
        CurrentState.RemoveProduct(productId);

    public void Start() =>
        CurrentState.Start();

    public string[] GetAllowedActions() =>
        CurrentState.AllowedActions;
}
