using System.Collections.Concurrent;

public interface ICacheable<K, V>
{
    public void Put(K key, V value);
    public V? Get(K key);
}

public class LRUCache<K, V>(int capacity) : ICacheable<K, V>
{
    private readonly ConcurrentDictionary<K, LinkedListNode<(K key, V value)>> _cache
        = new();
    private readonly LinkedList<(K key, V value)> _order = new();
    private readonly ReaderWriterLockSlim _rwLock = new(
        LockRecursionPolicy.NoRecursion);

    public V? Get(K key)
    {
        _rwLock.EnterUpgradeableReadLock();
        try
        {
            if (!_cache.TryGetValue(key, out var node))
                return default;
            _rwLock.EnterWriteLock();
            try
            {
                _order.Remove(node);
                _order.AddFirst(node);
            }
            finally { _rwLock.ExitWriteLock(); }

            return node.Value.value;
        }
        finally
        {
            _rwLock.ExitUpgradeableReadLock();
        }
    }

    public void Put(K key, V value)
    {
        _rwLock.EnterWriteLock();
        try
        {
            if (_cache.TryGetValue(key, out var existing))
            {
                _order.Remove(existing);
                _cache.TryRemove(key, out _);
            }
            else if (_cache.Count >= capacity)
            {
                var lru = _order.Last!;
                _order.RemoveLast();
                _cache.TryRemove(lru.Value.key, out _);
            }

            var node = new LinkedListNode<(K, V)>((key, value));
            _order.AddFirst(node);
            _cache[key] = node;
        }
        finally { _rwLock.ExitWriteLock(); }
    }
}
