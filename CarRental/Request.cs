public class Request(Car car, Customer customer, DateTime requestDate, DateTime returnDate)
{
    public Car Car { get; } = car;
    public Customer Customer { get; } = customer;
    public DateTime RequestDate { get; } = requestDate;
    public DateTime ReturnDate { get; } = returnDate;
    public bool IsPaid { get; private set; } = false;

    public int GetDays()
    {
        return (ReturnDate - RequestDate).Days + 1;
    }

    public decimal GetTotalPrice()
    {
        return Car.RentPerDay * GetDays();
    }

    public void Pay()
    {
        IsPaid = true;
    }

    public override string ToString()
    {
        return $"Request: {Car} for {Customer} from {RequestDate.ToShortDateString()} to {ReturnDate.ToShortDateString()}";
    }
}
