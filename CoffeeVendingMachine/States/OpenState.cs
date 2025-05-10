namespace VendingMachine.States;

public class OpenState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["Done", "AddIngredient", "AddNewIngredient", "CollectMoney", "AddNewProduct"];

    public override void Done() =>
        Context.CurrentState = StateFactory.Get<IdleState>(Context);

    public override void AddIngredient(int ingredientId, int quantity) =>
        Context.Repository.AddIngredient(ingredientId, quantity);

    public override int AddNewIngredient(string name, int priceInCents) =>
        Context.Repository.AddNewIngredient(name, priceInCents);

    public override int AddNewProduct(string name, Step[] steps, double feePercentage) =>
        Context.Repository.AddNewProduct(name, steps, feePercentage);
        
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
