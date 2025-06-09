public interface IReserve
{
    public bool AddRequest(Request request);
    public void CancelRequest(Request request);
}

public class ReserveService(CarRentalSystem repo) : IReserve
{
    public bool AddRequest(Request request)
    {
        if (request.ReturnDate <= request.RequestDate)
        {
            Console.WriteLine($"Invalid {request}: Return date must be after request date.");
            return false;
        }
        if (request.Customer.AddRequest(request) && request.Car.Reserve(request))
        {
            repo.Requests.Add(request);
            Console.WriteLine($"{request} added successfully.");
            return true;
        }
        request.Customer.CancelRequest(request);
        Console.WriteLine($"Failed to add {request}: Customer or car reservation failed.");
        return false;
    }

    public void CancelRequest(Request request)
    {
        if (request.IsPaid)
        {
            Console.WriteLine($"Cannot cancel {request} as it is already paid.");
            return;
        }
        if (repo.Requests.TryTake(out var r))
        {
            request.Customer.CancelRequest(request);
            request.Car.Cancel(request);
            Console.WriteLine($"{request} cancelled successfully.");
        }
        else
        {
            Console.WriteLine($"{request} not found for cancellation.");
        }
    }
}
