using System.Collections.Concurrent;

public interface ITaskRepository
{
    public TaskEntity CreateTask(string title, string description, DateTime? dueDate);
    public bool UpateTask(TaskEntity task, string title, string description, Status status, Priority? priority, DateTime? dueDate);
    public bool DeleteTask(TaskEntity task);
    public TaskEntity[] Search(TaskFilter filter);
    public void MarkCompleted(User user, TaskEntity task);
}

public record TaskFilter(Priority? priority, DateTime? minDate, DateTime? maxDate, HashSet<int> assignedUsers);

public class TaskRepository : ITaskRepository
{
    public ConcurrentDictionary<int, TaskEntity> tasks { get; } = [];

    public TaskEntity CreateTask(string title, string description, DateTime? dueDate)
    {
        var task = new TaskEntity(title, description, dueDate);
        tasks.TryAdd(task.Id, task);
        return task;
    }

    public bool DeleteTask(TaskEntity task)
    {
        if (tasks.Remove(task.Id, out var existingTask))
        {
            existingTask.AssignedUser?.Tasks.Remove(existingTask);
            return true;
        }
        return false;
    }

    public void MarkCompleted(User user, TaskEntity task)
    {
        if (!tasks.TryGetValue(task.Id, out TaskEntity? existingTask))
        {
            Console.WriteLine("task not found");
            return;
        }
        if (existingTask.AssignedUser?.Id != user.Id)
        {
            Console.WriteLine("not authorized");
            return;
        }
        existingTask.Status = Status.COMPLETED;
    }

    public TaskEntity[] Search(TaskFilter filter)
        => tasks.Values
            .Where(f => filter.priority is null || f.Priority == filter.priority)
            .Where(f => filter.minDate is null || f.DueDate is null || f.DueDate >= filter.minDate)
            .Where(f => filter.maxDate is null || f.DueDate is null || f.DueDate <= filter.maxDate)
            .Where(f => filter.assignedUsers is null || f.AssignedUser is null || filter.assignedUsers.Contains(f.AssignedUser.Id))
            .ToArray();

    public bool UpateTask(TaskEntity task, string title, string description, Status status, Priority? priority, DateTime? dueDate)
    {
        if (!tasks.TryGetValue(task.Id, out TaskEntity? existingTask))
        {
            Console.WriteLine("task not found");
            return false;
        }
        existingTask.Title = title;
        existingTask.Description = description;
        existingTask.Status = status;
        existingTask.Priority = priority;
        existingTask.DueDate = dueDate;
        return true;
    }
}
