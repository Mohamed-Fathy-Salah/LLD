public abstract class Vehicle(string plateNumber)
{
    public string PlateNumber { get; } = plateNumber;

    public override string ToString() => $"{this.GetType()} with plateNumber:{PlateNumber}";
}

public class Car(string plateNumber) : Vehicle(plateNumber) { }
public class MotorCycle(string plateNumber) : Vehicle(plateNumber) { }
public class Truck(string plateNumber) : Vehicle(plateNumber) { }
