var elevatorManager = new ElevatorManager(2, 600_000);
var r = new Random();
var tasks = new List<Task>();

for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Run(() =>
    {
        int floor = r.Next(0, 11);
        int floor2 = r.Next(0, 11);
        var direction = floor2 > floor ? DirectionEnum.UP : DirectionEnum.DOWN;

        var elevator = elevatorManager.RequestElevator(floor, direction);
        Console.WriteLine($"[Request] From {floor} → Elevator #{elevator.Id}");

        // Poll less aggressively or use an event/callback system in future
        while (elevator.CurrentFloor != floor)
            Thread.Sleep(200);

        elevator.GoToFloor(floor2);
        Console.WriteLine($"[Request] To {floor2} → Elevator #{elevator.Id}");
    }));
}

// Wait for all tasks to complete
Task.WaitAll(tasks.ToArray());
Console.ReadLine();
