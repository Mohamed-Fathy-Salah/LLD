namespace VendingMachine.States;

public class DispensingState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["Done"];

    public override void Done()
    {
        Console.WriteLine(string.Join("\n", Context.ProductQuantityInCart.Select(f => $"productid:{f.Key} quantity:{f.Value}")));
        Context.ProductQuantityInCart.Clear();
        Context.CurrentState = StateFactory.Get<ReturningChangeState>(Context);
    }
}
