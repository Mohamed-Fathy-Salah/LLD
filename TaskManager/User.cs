public class User(string name, string email)
{
    private static int _nextId = 1;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public List<TaskEntity> Tasks { get; } = [];
}
