using System.Collections.Concurrent;

public class AuthService
{
    private ConcurrentDictionary<string, User> _users = new();//email-user

    public User? SignIn(string email, string password)
    {
        if (_users.TryGetValue(email, out var user) && user.IsCorrectPassword(password))
            return user;
        Console.WriteLine("Invalid email or password.");
        return null;
    }

    public User? SignUp(string name, string email, string password)
    {
        if (_users.ContainsKey(email))
        {
            Console.WriteLine("Email already registered.");
            return null;
        }

        var user = new User(name, email, password);
        _users[email] = user;
        return user;
    }
}
