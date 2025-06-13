using System.Collections.Concurrent;

public class AuthService
{
    private static Lazy<AuthService> _instance = new Lazy<AuthService>(() => new AuthService());
    private AuthService() { }
    public static AuthService Instance => _instance.Value;
    private ConcurrentDictionary<string, User> _users = new();

    public User? SignIn(string email, string password)
    {
        if (_users.TryGetValue(email, out var user))
        {
            if (user.IsCorrectPassword(password))
            {
                return user;
            }
            Console.WriteLine("Incorrect password");
            return null;
        }
        Console.WriteLine($"User with email {email} not found");
        return null;
    }

    public User SignUp(string name, string email, string password)
    {
        if (_users.ContainsKey(email))
        {
            Console.WriteLine($"User with email {email} already exists");
            return _users[email];
        }

        var user = new User(name, email, password);
        _users[email] = user;
        Console.WriteLine($"User {name} signed up successfully");
        return user;
    }
}
