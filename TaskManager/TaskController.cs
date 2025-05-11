
public class TaskController(IUserRepository userRepository, ITaskRepository taskRepository, INotify notify)
{
    public void AssignTask(User user, TaskEntity task)
    {
        userRepository.AssignTask(user, task);
        notify.Cancel(task);
        notify.Notify(task, user);
    }

    public TaskEntity CreateTask(string title, string description, DateTime? dueDate)
        => taskRepository.CreateTask(title, description, dueDate);

    public User CreateUser(string name, string email)
        => userRepository.CreateUser(name, email);

    public bool DeleteTask(TaskEntity task)
    {
        notify.Cancel(task);
        return taskRepository.DeleteTask(task);
    }

    public bool DeleteUser(User user)
    {
        foreach (var task in user.Tasks)
            task.AssignedUser = null;
        user.Tasks.Clear();
        return userRepository.DeleteUser(user);
    }

    public void MarkCompleted(User user, TaskEntity task)
    {
        notify.Cancel(task);
        taskRepository.MarkCompleted(user, task);
    }

    public TaskEntity[] Search(TaskFilter filter)
        => taskRepository.Search(filter);

    public bool UpateTask(TaskEntity task, string title, string description, Status status, Priority? priority, DateTime? dueDate)
        => taskRepository.UpateTask(task, title, description, status, priority, dueDate);

    public bool UpdateUser(User user, string name, string email)
        => userRepository.UpdateUser(user, name, email);
}
