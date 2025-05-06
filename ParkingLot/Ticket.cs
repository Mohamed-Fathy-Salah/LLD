public class Ticket(Level level, Spot spot)
{
    public Guid ID { get; } = new Guid();
    public DateTime Time { get; } = DateTime.UtcNow;
    public Level Level { get; } = level;
    public Spot Spot { get; } = spot;

    public override string ToString() => $"Ticket created at {Time} in {Level} in {Spot} for {Spot.Vehicle}";
}
