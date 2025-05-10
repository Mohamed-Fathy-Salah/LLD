namespace VendingMachine.States;

public class PreparingState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["Done"];

    public override void Done()
    {
        foreach (var productQuantity in Context.ProductQuantityInCart)
        {
            var product = Context.Repository.GetProduct(productQuantity.Key);
            Console.WriteLine($"preparing {productQuantity.Value} {product}");
            foreach (var step in product.Steps)
                step.Execute();
        }
        Context.CurrentState = StateFactory.Get<DispensingState>(Context);
    }
}
