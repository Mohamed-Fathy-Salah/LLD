namespace VendingMachine.States;

public class AcceptingProductsState(VendingMachine context) : VendingMachineState(context)
{
    public override string[] AllowedActions => ["AddProduct", "RemoveProduct", "Done", "Cancel"];

    public override void AddProduct(int productId, int quantity)
    {
        if (Context.Repository.DisposeProductIngredients(productId, quantity))
        {
            if (Context.ProductQuantityInCart.TryGetValue(productId, out var existingQty))
                Context.ProductQuantityInCart[productId] = existingQty + quantity;
            else
                Context.ProductQuantityInCart[productId] = quantity;
        }
    }

    public override void RemoveProduct(int productId)
    {
        if (Context.ProductQuantityInCart.Remove(productId, out int quantity))
            Context.Repository.ReturnProductIngredients(productId, quantity);
    }

    public override void Done()
    {
        Context.CurrentState = StateFactory.Get<AcceptingMoneyState>(Context);
        Context.AmountToBePaidInCents = Context.ProductQuantityInCart.Sum(f => Context.Repository.GetProductPriceInCents(f.Key, f.Value));
        Console.WriteLine($"amount to be paid = {Context.AmountToBePaidInCents / 100.0}$");
    }

    public override void Cancel()
    {
        foreach (var (productId, quantity) in Context.ProductQuantityInCart)
            Context.Repository.ReturnProductIngredients(productId, quantity);
        Context.ProductQuantityInCart.Clear();
        Context.CurrentState = StateFactory.Get<IdleState>(Context);
    }
}
