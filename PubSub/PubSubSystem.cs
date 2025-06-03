using System.Collections.Concurrent;

public class PubSubSystem
{
    private readonly ConcurrentBag

    public void publish(string topic, string message)
    {
        if (topics.TryGetValue(topic, out var subscribers))
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.Receive(message);
            }
        }
    }

    public void subscribe(string topic, Subscriber subscriber) {
        topics.AddOrUpdate(topic, 
            _ => new HashSet<Subscriber> { subscriber }, 
            (_, existingSubscribers) => {
                existingSubscribers.Add(subscriber);
                return existingSubscribers;
            });
    }

    public void unsubscribe(string topic, Subscriber subscriber) {
        if (topics.TryGetValue(topic, out var subscribers))
        {
            subscribers.Remove(subscriber);
            if (subscribers.Count == 0)
            {
                topics.TryRemove(topic, out _);
            }
        }
    }
}
