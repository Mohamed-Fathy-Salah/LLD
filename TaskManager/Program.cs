// 1) Instantiate infrastructure
IUserRepository userRepo = new UserRepository();
ITaskRepository taskRepo = new TaskRepository();
INotify notifier = new Email();
var controller = new TaskController(userRepo, taskRepo, notifier);

// 2) Create a user
var alice = controller.CreateUser("Alice", "alice@example.com");
Console.WriteLine($"Created User: {alice.Id} – {alice.Name}");

// 3) Create a task
var due = DateTime.Now.AddDays(3);
var task = controller.CreateTask("Write report", "Weekly status report", due);
Console.WriteLine($"Created Task: {task.Id} – {task.Title} (due {task.DueDate})");

// 4) Assign it to Alice (schedules an email reminder)
controller.AssignTask(alice, task);
Console.WriteLine($"Assigned Task {task.Id} to User {alice.Id}");

// 5) Update the task’s status and priority
controller.UpateTask(task, task.Title, task.Description, Status.IN_PROGRESS, Priority.HIGH, task.DueDate);
Console.WriteLine($"Updated Task {task.Id}: status={task.Status}, priority={task.Priority}");

// 6) Search for all HIGH-priority tasks due in next week
var filter = new TaskFilter(Priority.HIGH, DateTime.Now, DateTime.Now.AddDays(7), new() { alice.Id });
var results = controller.Search(filter);
Console.WriteLine($"Search found {results.Length} task(s):");
foreach (var t in results)
    Console.WriteLine($" - [{t.Status}] {t.Title} (assigned to {t.AssignedUser?.Name})");

// 7) Mark it completed (also cancels reminder)
controller.MarkCompleted(alice, task);
Console.WriteLine($"Marked Task {task.Id} as {task.Status}");

// 8) Clean up
controller.DeleteTask(task);
controller.DeleteUser(alice);
Console.WriteLine("Cleanup done.");
