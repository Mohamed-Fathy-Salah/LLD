public class TaskEntity(string title, string description, DateTime? dueDate)
{
    private static int _nextId = 1;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public Status Status { get; set; } = Status.NEW;
    public Priority? Priority { get; set; }
    public DateTime? DueDate { get; set; } = dueDate;
    public User? AssignedUser { get; set; }
}
