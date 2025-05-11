using System.Collections.Concurrent;

public record SignalColor(byte red, byte green, byte blue);

public record RedSignal() : SignalColor(255, 0, 0);
public record GreenSignal() : SignalColor(0, 255, 0);
public record YellowSignal() : SignalColor(255, 255, 0);

public static class SignalColorFactory
{
    private static readonly ConcurrentDictionary<Type, SignalColor> _cache
        = new ConcurrentDictionary<Type, SignalColor>();

    public static T Get<T>() where T : SignalColor, new() =>
         (T)_cache.GetOrAdd(typeof(T), _ => new T());
}
