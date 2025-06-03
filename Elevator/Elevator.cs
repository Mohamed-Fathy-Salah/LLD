using C5;

public class Elevator
{
    private static int _nextId = 1;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public int CapacityInGrams { get; }
    public int CurrentFloor { get; private set; } = 0;
    public DirectionEnum CurrentDirection { get; private set; } = DirectionEnum.UP;
    private TreeSet<int> _requestedFloors { get; } = new TreeSet<int>();
    private WeightSensor _sensor { get; }

    public Elevator(int capacityInGrams, WeightSensor sensor)
    {
        if (capacityInGrams <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");
        CapacityInGrams = capacityInGrams;
        _sensor = sensor;

        new Thread(Run) { IsBackground = true }.Start();
    }

    public bool CanMove() =>
         _sensor.GetWeightInGrams() <= CapacityInGrams;

    public void GoToFloor(int floor)
    {
        if (floor < 0)
        {
            Console.WriteLine("Floor cannot be negative.");
            return;
        }
        lock (_requestedFloors)
        {
            _requestedFloors.Add(floor);
        }
    }

    private void Run()
    {
        Console.WriteLine($"starting elevator {Id}");
        while (true)
        {
            int nextFloor;
            lock (_requestedFloors)
            {
                if (_requestedFloors.Count == 0)
                    continue;


                while (!CanMove())
                {
                    Console.WriteLine($"Elevator {Id}: some of you needs to GTFO");
                    Thread.Sleep(100);
                }

                // if moving up get the next floor in the list bigger than the current floor
                // if moving down get the last floor in the list smaller than the current floor
                // if Direction is up and no bigger floor change direction to down
                // if Direction is down and no smaller floor change direction to up
                if (CurrentDirection == DirectionEnum.UP)
                {
                    var view = _requestedFloors.RangeFrom(CurrentFloor);
                    if (view.Count > 0)
                        nextFloor = view.First();
                    else
                    {
                        nextFloor = _requestedFloors.RangeTo(CurrentFloor).Last();
                        CurrentDirection = DirectionEnum.DOWN;
                    }
                }
                else
                {
                    var view = _requestedFloors.RangeTo(CurrentFloor);
                    if (view.Count > 0)
                        nextFloor = view.Last();
                    else
                    {
                        nextFloor = _requestedFloors.RangeFrom(CurrentFloor).First();
                        CurrentDirection = DirectionEnum.UP;
                    }
                }
            }

            while (CurrentFloor != nextFloor)
            {
                if (CurrentFloor < nextFloor)
                    CurrentFloor++;
                else
                    CurrentFloor--;
                Console.WriteLine($"Elevator {Id}: floor {CurrentFloor} and going {CurrentDirection.ToString()}");
                Thread.Sleep(100);
            }

            lock (_requestedFloors)
            {
                _requestedFloors.Remove(CurrentFloor);
                Console.WriteLine($"Elevator {Id}: done {CurrentFloor}");
            }
        }
    }

    public int RequestsCount
    {
        get
        {
            lock (_requestedFloors)
            {
                return _requestedFloors.Count;
            }
        }
    }
}
