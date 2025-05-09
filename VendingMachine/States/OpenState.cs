namespace VendingMachine.States;

public class OpenState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["Done", "AddProduct", "AddNewProduct", "CollectMoney"];

    public override void Done() =>
        Context.CurrentState = StateFactory.Get<IdleState>(Context);

    public override void AddProduct(int productId, int quantity) =>
        Context.Repository.AddProduct(productId, quantity);

    public override int AddNewProduct(string name, int priceInCents) =>
        Context.Repository.AddNewProduct(name, priceInCents);

    public override void CollectMoney(int amount)
    {
        if (Context.AmountInCents < amount)
        {
            Console.WriteLine("no enough money in the machine!");
            return;
        }
        Context.AmountInCents -= amount;
    }
}
