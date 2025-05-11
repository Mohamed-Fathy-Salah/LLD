using System.Collections.Concurrent;

public interface IUserRepository
{
    public User CreateUser(string name, string email);
    public bool UpdateUser(User user, string name, string email);
    public bool DeleteUser(User user);
    public void AssignTask(User user, TaskEntity task);
}

public class UserRepository() : IUserRepository
{
    public ConcurrentDictionary<int, User> users { get; } = [];

    public void AssignTask(User user, TaskEntity task)
    {
        if (!users.TryGetValue(user.Id, out User? existingUser))
        {
            Console.WriteLine("user not found!");
            return;
        }
        existingUser.Tasks.Add(task);
        task.AssignedUser = existingUser;
    }

    public User CreateUser(string name, string email)
    {
        var user = new User(name, email);
        users.TryAdd(user.Id, user);
        return user;
    }

    public bool DeleteUser(User user) =>
        users.Remove(user.Id, out var _);

    public bool UpdateUser(User user, string name, string email)
    {
        if (!users.TryGetValue(user.Id, out User? existingUser))
        {
            Console.WriteLine("user not found!");
            return false;
        }
        existingUser.Name = name;
        existingUser.Email = email;
        return true;
    }
}
