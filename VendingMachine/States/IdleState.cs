namespace VendingMachine.States;

public class IdleState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["OpenMachine", "Start"];

    public override void OpenMachine() => Context.CurrentState = StateFactory.Get<OpenState>(Context);
    public override void Start() => Context.CurrentState = StateFactory.Get<AcceptingProductsState>(Context);
}
