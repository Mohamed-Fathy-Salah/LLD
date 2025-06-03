public class Subscriber
{
    private static int nextId = 0;
    public int id { get; } = Interlocked.Increment(ref nextId);
    public void Receive(string message)
    {
        Console.WriteLine($"Subscriber {id} received message: {message}");
    }
}
