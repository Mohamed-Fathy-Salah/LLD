public interface ILevelObserver
{
    public void update(Level l);
}

public class DisplayBoard : ILevelObserver
{
    public void update(Level l)
    {
        Console.WriteLine($"updating {l}\n" + string.Join("\n",
                    l.Spots
                    .Select(f => $"{f} has {f.Vehicle}")));
    }
}
