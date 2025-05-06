public abstract class Spot(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public Vehicle? Vehicle { get; protected set; }
    private object _lock = new object();

    public bool IsEmpty()
    {
        return Vehicle is null;
    }

    public bool TryAssignVehicle(Vehicle v)
    {
        if (!CanFitVehicle(v)) return false;
        lock (_lock)
        {
            if (Vehicle is null)
            {
                Vehicle = v;
                return true;
            }
        }
        return false;
    }

    public bool Free()
    {
        if (IsEmpty()) return false;
        Vehicle = null;
        return true;
    }

    public abstract bool CanFitVehicle(Vehicle v);

    public override string ToString() => $"{this.GetType()} {X},{Y}";
}

public class MotorCycleSpot(int x, int y) : Spot(x, y)
{
    public override bool CanFitVehicle(Vehicle v) => v is MotorCycle;
}

public class CarSpot(int x, int y) : Spot(x, y)
{
    public override bool CanFitVehicle(Vehicle v) => v is Car or MotorCycle;
}

public class TruckSpot(int x, int y) : Spot(x, y)
{
    public override bool CanFitVehicle(Vehicle v) => v is Car or MotorCycle or Truck;
}
