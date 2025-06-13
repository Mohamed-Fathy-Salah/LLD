using System.Collections.Concurrent;

// Initialize services
var authService = AuthService.Instance;
var wallet = DigitalWallet.Instance;

// Create users
var users = new[]
{
    authService.SignUp("Alice", "alice@example.com", "password123"),
    authService.SignUp("Bob", "bob@example.com", "password456"),
    authService.SignUp("Charlie", "charlie@example.com", "password789"),
    authService.SignUp("Diana", "diana@example.com", "passwordabc")
};

// Create payment methods for each user
var random = new Random();
var paymentMethods = new ConcurrentBag<Payment>();

foreach (var user in users)
{
    // Create random payment methods for each user
    var currencies = Enum.GetValues<CurrencyEnum>();
    
    // Add a credit card
    var creditCardCurrency = currencies[random.Next(currencies.Length)];
    var creditCardBalance = random.Next(1000, 5000);
    var creditCard = new CreditCardPayment(user, creditCardBalance, creditCardCurrency);
    user.Payments.Add(creditCard);
    paymentMethods.Add(creditCard);
    
    // Add a bank account
    var bankAccountCurrency = currencies[random.Next(currencies.Length)];
    var bankAccountBalance = random.Next(2000, 8000);
    var bankAccount = new BankAccountPayment(user, bankAccountBalance, bankAccountCurrency);
    user.Payments.Add(bankAccount);
    paymentMethods.Add(bankAccount);
}

Console.WriteLine("=== Digital Wallet System Demo ===\n");

// Display initial balances
Console.WriteLine("Initial Balances:");
foreach (var user in users)
{
    Console.WriteLine($"{user}:");
    foreach (var payment in user.Payments)
    {
        Console.WriteLine($"  {payment}: {payment.Balance} {payment.Currency}");
    }
}
Console.WriteLine();

// Update some exchange rates
Console.WriteLine("Updating exchange rates...");
CurrencyExchage.SetExchangeRate(CurrencyEnum.USD, CurrencyEnum.EGP, 32.00m);
CurrencyExchage.SetExchangeRate(CurrencyEnum.EUR, CurrencyEnum.USD, 1.12m);
Console.WriteLine("Exchange rates updated.\n");

// Create tasks for concurrent transactions
var tasks = new List<Task>();
var completedTransactions = new ConcurrentBag<bool>();

// Each user will perform transactions on their own thread
foreach (var user in users)
{
    var task = Task.Run(async () =>
    {
        var userPayments = paymentMethods.Where(p => p.Owner == user).ToArray();
        var otherPayments = paymentMethods.Where(p => p.Owner != user).ToArray();
        
        // Perform multiple transactions for this user
        for (int i = 0; i < 3; i++)
        {
            await Task.Delay(random.Next(100, 500)); // Simulate some processing time
            
            if (userPayments.Length > 0 && otherPayments.Length > 0)
            {
                var senderPayment = userPayments[random.Next(userPayments.Length)];
                var receiverPayment = otherPayments[random.Next(otherPayments.Length)];
                var amount = random.Next(50, 300);
                var currency = Enum.GetValues<CurrencyEnum>()[random.Next(3)];
                
                Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] {user.Name} attempting to send {amount} {currency}...");
                
                var success = wallet.TryTransfer(senderPayment, receiverPayment, amount, currency);
                completedTransactions.Add(success);
                
                if (!success)
                {
                    Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Transaction failed - insufficient funds");
                }
            }
        }
    });
    
    tasks.Add(task);
}

// Wait for all transactions to complete
await Task.WhenAll(tasks);

Console.WriteLine("\n=== Transaction Summary ===");
var successfulTransactions = completedTransactions.Count(t => t);
var failedTransactions = completedTransactions.Count(t => !t);
Console.WriteLine($"Successful transactions: {successfulTransactions}");
Console.WriteLine($"Failed transactions: {failedTransactions}");

// Display final balances
Console.WriteLine("\nFinal Balances:");
foreach (var user in users)
{
    Console.WriteLine($"{user}:");
    foreach (var payment in user.Payments)
    {
        Console.WriteLine($"  {payment}: {payment.Balance} {payment.Currency}");
    }
}

// Display transaction history for each user
Console.WriteLine("\n=== Transaction History ===");
foreach (var user in users)
{
    var userTransactions = wallet.GetTransactions(user);
    Console.WriteLine($"\n{user} Transaction History ({userTransactions.Length} transactions):");
    
    foreach (var transaction in userTransactions.OrderBy(t => t.CreatedAt))
    {
        var direction = transaction.Sender.Owner == user ? "SENT" : "RECEIVED";
        var otherParty = transaction.Sender.Owner == user ? transaction.Receiver.Owner.Name : transaction.Sender.Owner.Name;
        var status = transaction.IsCompleted ? "✓" : "✗";
        
        Console.WriteLine($"  {status} {direction} {transaction.Amount} {transaction.Currency} {(direction == "SENT" ? "to" : "from")} {otherParty} at {transaction.CreatedAt:HH:mm:ss.fff}");
    }
}

// Demonstrate authentication
Console.WriteLine("\n=== Authentication Test ===");
var signInTask1 = Task.Run(() =>
{
    Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Attempting to sign in Alice...");
    var signedInUser = authService.SignIn("alice@example.com", "password123");
    Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Sign in result: {(signedInUser != null ? "Success" : "Failed")}");
});

var signInTask2 = Task.Run(() =>
{
    Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Attempting to sign in with wrong password...");
    var signedInUser = authService.SignIn("bob@example.com", "wrongpassword");
    Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Sign in result: {(signedInUser != null ? "Success" : "Failed")}");
});

await Task.WhenAll(signInTask1, signInTask2);

// Test currency conversion
Console.WriteLine("\n=== Currency Conversion Test ===");
try
{
    var usdAmount = 100m;
    var egpEquivalent = CurrencyExchage.Convert(CurrencyEnum.USD, CurrencyEnum.EGP, usdAmount);
    var eurEquivalent = CurrencyExchage.Convert(CurrencyEnum.USD, CurrencyEnum.EUR, usdAmount);
    
    Console.WriteLine($"{usdAmount} USD = {egpEquivalent} EGP");
    Console.WriteLine($"{usdAmount} USD = {eurEquivalent} EUR");
}
catch (Exception ex)
{
    Console.WriteLine($"Currency conversion error: {ex.Message}");
}

Console.WriteLine("\n=== Demo Complete ===");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
