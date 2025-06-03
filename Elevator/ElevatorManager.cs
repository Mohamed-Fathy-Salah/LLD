public class ElevatorManager
{
    private Elevator[] Elevators { get; }

    public ElevatorManager(int elevatorCount, int capacityInGrams)
    {
        if (elevatorCount <= 0)
            throw new Exception("bruh");
        Elevators = Enumerable.Range(0, elevatorCount).Select(f => new Elevator(capacityInGrams, new WeightSensor())).ToArray();
    }

    public Elevator RequestElevator(int fromFloor, DirectionEnum direction)
    {
        var elevator = Elevators.MinBy(f => GetScore(fromFloor, direction, f));
        elevator!.GoToFloor(fromFloor);
        return elevator;
    }

    private int GetScore(int currentFloor, DirectionEnum direction, Elevator elevator)
    {
        bool isAbove = currentFloor <= elevator.CurrentFloor;
        bool isGoingUp = direction == DirectionEnum.UP;
        bool isElevatorGoingUp = elevator.CurrentDirection == DirectionEnum.UP;

        int score = 0;
        if ((isGoingUp && !isAbove && isElevatorGoingUp) || (!isGoingUp && isAbove && !isElevatorGoingUp))
            score = 1;
        else if ((isGoingUp && isAbove && !isElevatorGoingUp) || (!isGoingUp && !isAbove && isElevatorGoingUp))
            score = 5;
        else
            score = 10;

        return score * Math.Abs(currentFloor - elevator.CurrentFloor) * elevator.RequestsCount;
    }
}
