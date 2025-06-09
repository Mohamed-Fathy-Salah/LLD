//driver program for the C# console application

var system = new CarRentalSystem();
var searchService = new SearchService(system);
var reserveService = new ReserveService(system);
var cashPayment = new CashPayment();
int counter = 0;

system.setServices(searchService, reserveService);

for (int i = 0; i < 10; i++)
{
    var car = new Car((CarTypeEnum)(i % 3), "Camry", 2020, "adf", 100.0m + i * 10);
    system.Cars.Add(car);
}
void customerThread1()
{
    var random = new Random();
    var customer = new Customer("John Doe" + Interlocked.Increment(ref counter), "011", "adf");
    system.Customers.Add(customer);

    var cars = system.SearchService.Search(new Filter(type: (CarTypeEnum)(random.Next(3))));
    var car = cars[random.Next(cars.Count)];
    var request = new Request(car, customer, DateTime.Now.AddDays(random.Next(10) - 5), DateTime.Now.AddDays(random.Next(10) - 5 + 1));
    system.Requests.Add(request);
    Thread.Sleep(random.Next(100, 500));
    customer.Pay(request, cashPayment);
    Thread.Sleep(random.Next(100, 500));
    customer.Pay(request, cashPayment);
    Thread.Sleep(random.Next(100, 500));
    system.ReserveService.CancelRequest(request);
}

void customerThread2()
{
    var random = new Random();
    var customer = new Customer("John Doe" + Interlocked.Increment(ref counter), "011", "adf");
    system.Customers.Add(customer);

    var cars = system.SearchService.Search(new Filter(type: (CarTypeEnum)(random.Next(3))));
    var car = cars[random.Next(cars.Count)];
    var request = new Request(car, customer, DateTime.Now.AddDays(random.Next(10) - 5), DateTime.Now.AddDays(random.Next(10) - 5 + 1));
    system.Requests.Add(request);
    system.ReserveService.CancelRequest(request);
    Thread.Sleep(random.Next(100, 500));
    customer.Pay(request, cashPayment);
}

for (int i = 0; i < 5; i++)
{
    new Thread(customerThread1).Start();
    new Thread(customerThread2).Start();
}
