public abstract class SpotAllocationStrategy
{
    public Spot? Allocate(Level l, Vehicle v)
    {
        var spot = ChooseSpot(l, v);
        if (spot is not null) l.NotifyAll();
        return spot;
    }

    protected abstract Spot? ChooseSpot(Level l, Vehicle v);
}

public class DefaultAllocationStrategy : SpotAllocationStrategy
{
    protected override Spot? ChooseSpot(Level l, Vehicle v)
        => l.Spots.FirstOrDefault(f => f.TryAssignVehicle(v));
}

public class NearestFirstAllocationStrategy : SpotAllocationStrategy
{
    protected override Spot? ChooseSpot(Level l, Vehicle v)
    {
        throw new NotImplementedException();
    }
}

public class ZonedAllocationStrategy : SpotAllocationStrategy
{
    protected override Spot? ChooseSpot(Level l, Vehicle v)
    {
        throw new NotImplementedException();
    }
}
