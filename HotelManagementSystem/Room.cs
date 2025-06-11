public enum RoomTypeEnum
{
    SINGLE,
    DOUBLE,
    DELUX,
    SUITE
}

public enum ReservationStatusEnum
{
    AVAILABLE,
    RESERVED,
    CHECKED_IN
}

public class Room(string label, RoomTypeEnum type, decimal rentPerDay)
{
    public string Label { get; } = label;
    public RoomTypeEnum Type { get; } = type;
    public ReservationStatusEnum status { get; private set; } = ReservationStatusEnum.AVAILABLE;
    public decimal RentPerDay { get; } = rentPerDay;
    public Guest? AssignedGuest = null;
    private ReaderWriterLockSlim _lock = new();

    public void CheckIn()
    {
        try
        {
            _lock.EnterReadLock();
            if (AssignedGuest == null)
                return;
            status = ReservationStatusEnum.CHECKED_IN;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void CheckOut()
    {
        try
        {
            _lock.EnterReadLock();
            if (AssignedGuest == null)
                return;
            status = ReservationStatusEnum.RESERVED;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public bool AssignGuest(Guest guest)
    {
        try
        {
            _lock.EnterUpgradeableReadLock();
            if (AssignedGuest != null)
                return false;
            try
            {
                _lock.EnterWriteLock();
                AssignedGuest = guest;
                status = ReservationStatusEnum.RESERVED;
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
        return true;
    }

    public bool FreeRoom()
    {
        try
        {
            _lock.EnterUpgradeableReadLock();
            if (AssignedGuest == null)
                return false;
            try
            {
                _lock.EnterWriteLock();
                AssignedGuest = null;
                status = ReservationStatusEnum.AVAILABLE;
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
        return true;
    }
}
