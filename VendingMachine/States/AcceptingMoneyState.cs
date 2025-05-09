namespace VendingMachine.States;

public class AcceptingMoneyState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["InsertMoney", "Cancel"];

    public override void InsertMoney(int amountInCents)
    {
        if (amountInCents <= 0)
        {
            Console.WriteLine("bruh WTF are you doing?");
            return;
        }

        Context.PaidAmountInCents += amountInCents;
        if (Context.AmountToBePaidInCents <= Context.PaidAmountInCents)
            Context.CurrentState = StateFactory.Get<DispensingState>(Context);
        else
            Console.WriteLine($"remaining amount to be paid is {(Context.AmountToBePaidInCents - Context.PaidAmountInCents) / 100.0}$");
    }

    public override void Cancel()
    {
        Context.ProductQuantityInCart.Clear();
        Context.AmountToBePaidInCents = 0;
        Context.CurrentState = StateFactory.Get<ReturningChangeState>(Context);
    }
}
