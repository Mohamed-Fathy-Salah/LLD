int capacity = 3;
var cache = new LRUCache<int, string>(capacity);

var tasks = new List<Task>();

// Thread 1: Put values
tasks.Add(Task.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
        cache.Put(i % 5, $"Value-{i}");
        Console.WriteLine($"[Put] Key: {i % 5}, Value: Value-{i}");
        Thread.Sleep(50);
    }
}));

// Thread 2: Get values
tasks.Add(Task.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
        var key = i % 5;
        var value = cache.Get(key);
        Console.WriteLine($"[Get] Key: {key}, Value: {value ?? "null"}");
        Thread.Sleep(70);
    }
}));

// Thread 3: Random mix
tasks.Add(Task.Run(() =>
{
    var rand = new Random();
    for (int i = 0; i < 10; i++)
    {
        int key = rand.Next(0, 5);
        if (rand.NextDouble() < 0.5)
        {
            cache.Put(key, $"R-{i}");
            Console.WriteLine($"[Put] Key: {key}, Value: R-{i}");
        }
        else
        {
            var val = cache.Get(key);
            Console.WriteLine($"[Get] Key: {key}, Value: {val ?? "null"}");
        }
        Thread.Sleep(60);
    }
}));

Task.WaitAll(tasks.ToArray()); // Ensure the program waits for all threads
Console.WriteLine("All threads completed.");
