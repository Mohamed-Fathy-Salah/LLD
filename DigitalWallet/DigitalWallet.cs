using System.Collections.Concurrent;

public class DigitalWallet
{
    private static Lazy<DigitalWallet> _instance = new Lazy<DigitalWallet>(() => new DigitalWallet());
    private DigitalWallet() { }
    public static DigitalWallet Instance => _instance.Value;
    private ConcurrentBag<Transaction> _transactions = new();

    public Transaction[] GetTransactions(User user)
    {
        return _transactions
            .Where(t => t.Sender.Owner == user || t.Receiver.Owner == user)
            .ToArray();
    }

    public bool TryTransfer(Payment sender, Payment receiver, decimal amount, CurrencyEnum currency)
    {
        Payment[] lockOrder = (sender.Id <= receiver.Id) ? [sender, receiver] : [receiver, sender];
        lock (lockOrder[0])
        {
            lock (lockOrder[1])
            {
                var transaction = new Transaction(sender, receiver, amount, currency);
                if (sender.TryTakeBalance(CurrencyExchage.Convert(currency, sender.Currency, amount)))
                {
                    receiver.AddBalance(CurrencyExchage.Convert(currency, receiver.Currency, amount));
                    transaction.Complete();
                    _transactions.Add(transaction);
                    return true;
                }
                _transactions.Add(transaction);
            }
        }
        return false;
    }
}
