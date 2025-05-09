namespace VendingMachine;

public interface IVendingMachineState
{
    public void Start();
    public void Done();
    public void Cancel();
    public void OpenMachine();
    public void CollectMoney(int amount);
    public int AddNewProduct(string name, int priceInCents);
    public void AddProduct(int productId, int quantity);
    public void RemoveProduct(int productId);
    public void InsertMoney(int amount);
}
