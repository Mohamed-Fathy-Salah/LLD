public interface IObserver
{
    void InCommingTransaction(Transaction transaction);
}

public class User : IObserver
{
    public string Name { get; }
    public string Email { get; }
    private string _hashedPassword { get; }
    public List<Payment> Payments { get; } = new List<Payment>();

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        _hashedPassword = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt());
    }

    public bool IsCorrectPassword(string password)
    {
        return DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(password, _hashedPassword);
    }

    public void InCommingTransaction(Transaction transaction)
    {
        Console.WriteLine($"{this} received {transaction}");
    }

    public override string ToString()
    {
        return $"User {Name}";
    }
}
