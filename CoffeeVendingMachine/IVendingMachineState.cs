namespace VendingMachine;

public interface IVendingMachineState
{
    public void Start();
    public void Done();
    public void Cancel();
    public void OpenMachine();
    public void CollectMoney(int amount);
    public int AddNewIngredient(string name, int priceInCents);
    public void AddIngredient(int ingredientId, int quantity);
    public int AddNewProduct(string name, Step[] steps, double feePercentage);
    public void AddProduct(int productId, int quantity);
    public void RemoveProduct(int productId);
    public void InsertMoney(int amount);
}
