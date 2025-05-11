public interface INotify
{
    public void Notify(TaskEntity task, User user);
    public void Cancel(TaskEntity task);
}

public class Email : INotify
{
    public Dictionary<int, Notification> notifications { get; } = [];

    public void Cancel(TaskEntity task)
    {
        notifications.Remove(task.Id);
    }

    public void Notify(TaskEntity task, User user)
    {
        if (task.DueDate is null)
            return;
        var notification = new Notification(task.Id, user.Id, task.DueDate.Value);
        notifications.Add(task.Id, notification);
    }
}

public class Notification(int taskId, int userId, DateTime time)
{
    public int TaskId { get; } = taskId;
    public int UserId { get; } = userId;
    public DateTime Time { get; } = time;
}
