public class Transaction (Payment sender, Payment receiver, decimal amount, CurrencyEnum currency){
    public Payment Sender {get;} = sender;
    public Payment Receiver {get;} = receiver;
    public decimal Amount { get; } = amount;
    public CurrencyEnum Currency { get; } = currency;
    public DateTime CreatedAt { get; } = DateTime.Now;
    public bool IsCompleted { get; private set; } = false;

    public void Complete()
    {
        if (IsCompleted)
        {
            Console.WriteLine($"{this} is already completed");
            return;
        }
        IsCompleted = true;
        Receiver.Owner.InCommingTransaction(this);
        Console.WriteLine($"{this} completed successfully");
    }

    public override string ToString()
    {
        return $"Transaction from {Sender} to {Receiver} of {Amount} {Currency} at {CreatedAt}";
    }
}
