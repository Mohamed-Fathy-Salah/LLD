public class Level(int levelNumber, List<Spot> spots, List<ILevelObserver> observers)
{
    public int LevelNumber { get; } = levelNumber;

    public List<Spot> Spots { get; } = spots;

    public List<ILevelObserver> Observers { get; set; } = observers;

    //todo: O(1)
    public bool HasEmptySpots(Vehicle v) => Spots.Any(f => f.CanFitVehicle(v) && f.IsEmpty());

    //todo: O(1)
    public Spot? TryAllocateSpot(Vehicle v)
    {
        var spot = Spots.FirstOrDefault(f => f.TryAssignVehicle(v));
        if (spot is not null) NotifyAll();
        return spot;
    }

    public bool FreeSpot(Spot s)
    {
        if (s.Free())
        {
            NotifyAll();
            return true;
        }
        return false;
    }

    public void NotifyAll()
    {
        foreach (var observer in Observers)
            observer.update(this);
    }

    public override string ToString() => $"Level {LevelNumber}";
}
