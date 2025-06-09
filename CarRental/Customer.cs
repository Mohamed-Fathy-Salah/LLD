public class Customer(string name, string phoneNumber, string licenceInformation)
{
    public string Name { get; } = name;
    public string PhoneNumber { get; } = phoneNumber;
    public string LicenceInformation { get; } = licenceInformation;
    private List<Request> _requests = new();

    public bool AddRequest(Request request)
    {
        if (_requests.Any(r => (request.RequestDate > r.RequestDate && request.RequestDate < r.ReturnDate) ||
                    (request.ReturnDate > r.RequestDate && request.ReturnDate < r.ReturnDate))) {
            Console.WriteLine($"Request {request} overlaps with an existing request.");
            return false;
        }
        _requests.Add(request);
        return true;
    }

    public void CancelRequest(Request request)
    {
        _requests.Remove(request);
        Console.WriteLine($"Request {request} cancelled for customer {Name}.");
    }

    public bool Pay(Request request, IPayment payment)
    {
        if (!_requests.Contains(request)) {
            Console.WriteLine($"Request {request} not found for customer {Name}.");
            return false;
        }

        if (request.IsPaid)
        {
            Console.WriteLine($"Request {request} is already paid.");
            return false;
        }

        if (payment.Pay(this, request.GetTotalPrice()))
        {
            request.Pay();
            Console.WriteLine($"Payment of {request.GetTotalPrice()} successful for request {request}.");
            return true;
        }

        Console.WriteLine($"Payment failed for request {request}.");
        return false;
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}
