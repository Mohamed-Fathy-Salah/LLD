public class User(string name)
{
    public string Name { get; } = name;
    public Buggage[] Baggages { get; } = [];

    public void FlightChanged(Flight flight)
    {
        Console.WriteLine($"User {Name} notified about flight change: {flight.Source} to {flight.Destination} at {flight.From}");
    }
}

public class Buggage(string description, double weight)
{
    public string Description { get; } = description;
    public double Weight { get; } = weight;

    public override string ToString()
    {
        return $"{Description} ({Weight} kg)";
    }
}
