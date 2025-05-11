public class Intersection(List<Road> roads)
{
    private static int _nextId = 1;
    public int Id { get; init; } = Interlocked.Increment(ref _nextId);
    public List<Road> Roads { get; init; } = roads;
}
