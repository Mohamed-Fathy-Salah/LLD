using System.Collections.Concurrent;

namespace VendingMachine;

public static class StateFactory
{
    private static readonly ConcurrentDictionary<(VendingMachine, Type), Lazy<VendingMachineState>> _cache
        = new();

    public static T Get<T>(VendingMachine context) where T : VendingMachineState
    {
        var key = (context, typeof(T));

        // Atomically get-or-create a Lazy<VendingMachineState> for this key
        var lazyWrapper = _cache.GetOrAdd(
            key,
            _ => new Lazy<VendingMachineState>(
                    () => (VendingMachineState)Activator.CreateInstance(typeof(T), context),
                    LazyThreadSafetyMode.ExecutionAndPublication
                 )
        );  

        return (T)lazyWrapper.Value;
    }

    internal static VendingMachineState? Get<T>()
    {
        throw new NotImplementedException();
    }
}
