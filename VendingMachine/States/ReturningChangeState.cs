namespace VendingMachine.States;

public class ReturningChangeState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["Done"];

    public override void Done()
    {
        Console.WriteLine($"pls take ur money {(Context.PaidAmountInCents - Context.AmountToBePaidInCents) / 100.0}$");
        Context.AmountInCents += Context.AmountToBePaidInCents;
        Context.PaidAmountInCents = 0;
        Context.AmountToBePaidInCents = 0;
        Context.CurrentState = new IdleState(Context);
    }
}
