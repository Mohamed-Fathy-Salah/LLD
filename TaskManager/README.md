# Designing a Task Management System

## Requirements
1. The task management system should allow users to create, update, and delete tasks.
2. Each task should have a title, description, due date, priority, and status (e.g., pending, in progress, completed).
3. Users should be able to assign tasks to other users and set reminders for tasks.
4. The system should support searching and filtering tasks based on various criteria (e.g., priority, due date, assigned user).
5. Users should be able to mark tasks as completed and view their task history.
6. The system should handle concurrent access to tasks and ensure data consistency.
7. The system should be extensible to accommodate future enhancements and new features.

```mermaid
classDiagram
    class TaskController {
        IUserRepository userRepo
        ITaskRepository taskRepo
        INotify notifications
        User CreateUser(name, email)
        bool UpdateUser(userId, name, email)
        bool DeleteUser(userId)
        void AssignTask(userId, taskId)
        Task CreateTask(title, description, status, priority, dueDate)
        bool UpdateTask(taskId, title, description, status, priority, dueDate)
        bool DeleteTask(taskId)
        Task[] Search(priority, minDate, maxDate, assignedUsers)
        void MarkCompleted(userId, taskId)
    }
    class IUserRepository {
        <<interface>>
        User CreateUser(name, email)
        bool UpdateUser(userId, name, email)
        bool DeleteUser(userId)
        void AssignTask(userId, taskId)
    }
    class UserRepository {
        Dict~int,User~ users
        User CreateUser(name, email)
        bool UpdateUser(userId, name, email)
        bool DeleteUser(userId)
        void AssignTask(userId, taskId)
    }
    class ITaskRepository {
        <<interface>>
        Task CreateTask(title, description, status, priority, dueDate)
        bool UpdateTask(taskId, title, description, status, priority, dueDate)
        bool DeleteTask(taskId)
        Task[] Search(TaskFilter)
        void MarkCompleted(userId, taskId)
    }
    class TaskRepository {
        Dict~int,Task~ tasks
        Task CreateTask(title, description, status, priority, dueDate)
        bool UpdateTask(taskId, title, description, status, priority, dueDate)
        bool DeleteTask(taskId)
        Task[] Search(TaskFilter)
        void MarkCompleted(userId, taskId)
    }
    class TaskFilter {
        Priority? priority
        DateTime? minDate
        DateTime? maxDate
        User[] assignedUsers
    }
    class INotify {
        int notify(userId, taskId, time)
        void cancel(notificationId)
    }
    class Notification {
        int userId
        int taskId
        DateTime time
    }
    class Email {
        Dict~int, Notification~ notifications
        int notify(userId, taskId, time)
        void cancel(notificationId)
    }
    class Task {
        int id
        string title
        string description
        Status status
        Priority? priority
        DateTime? dueDate
        User? assignedUser
    }
    class User {
        int id
        string name
        string email
        Task[] tasks
    }
    class Status {
        <<enumeration>>
        PENDING
        INPROGRESS
        COMPLETED
    }
    class Priority {
        <<enumeration>>
        HIGH
        MID
        LOW
    }
    TaskController --> IUserRepository
    TaskController --> ITaskRepository
    TaskController --> INotify
    ITaskRepository <.. TaskRepository
    IUserRepository <.. UserRepository
    UserRepository "1" -- "*" User
    TaskRepository "1" -- "*" Task
    User "1" -- "*" Task
    INotify <.. Email 
    Email "1" -- "*" Notification
```
