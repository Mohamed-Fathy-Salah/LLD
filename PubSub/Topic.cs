using System.Collections.Concurrent;

public class Topic(string name)
{
    private static int _nextId = 1;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public string Name { get; } = name;
    private readonly ConcurrentDictionary<int, Subscriber> Subscribers = new();
    public void Subscribe(Subscriber subscriber)
    {
        Subscribers.TryAdd(subscriber.id, subscriber);
    }
}
