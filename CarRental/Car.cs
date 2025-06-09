public class Car(CarTypeEnum type, string model, short year, string plateNumber, decimal rentPerDay)
{
    public CarTypeEnum Type { get; } = type;
    public string Model { get; } = model;
    public short Year { get; } = year;
    public string PlateNumber { get; } = plateNumber;
    public decimal RentPerDay { get; } = rentPerDay;
    private List<Request> _requests = new();
    private ReaderWriterLockSlim _lock = new();

    public bool IsAvailable()
    {
        return IsAvailable(DateTime.Now, DateTime.Now.AddDays(1));
    }

    public bool IsAvailable(DateTime from, DateTime to)
    {
        try
        {
            _lock.EnterReadLock();
            return IsAvailableNotSafe(from, to);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    private bool IsAvailableNotSafe(DateTime from, DateTime to)
    {
        return !_requests.Any(r => r.RequestDate < to && r.ReturnDate > from);
    }

    public bool Reserve(Request request)
    {
        _lock.EnterUpgradeableReadLock();
        try
        {
            if (!IsAvailableNotSafe(request.RequestDate, request.ReturnDate))
                return false;

            _lock.EnterWriteLock();
            try
            {
                _requests.Add(request);
                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        finally
        {
            _lock.ExitUpgradeableReadLock();
        }
    }

    public void Cancel(Request request)
    {
        try
        {
            _lock.EnterWriteLock();
            _requests.Remove(request);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public override string ToString()
    {
        return Type.ToString();
    }
}
