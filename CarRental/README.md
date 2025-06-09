# Designing a Car Rental System

## Requirements
1. The car rental system should allow customers to browse and reserve available cars for specific dates.
2. Each car should have details such as make, model, year, license plate number, and rental price per day.
3. Customers should be able to search for cars based on various criteria, such as car type, price range, and availability.
4. The system should handle reservations, including creating, modifying, and canceling reservations.
5. The system should keep track of the availability of cars and update their status accordingly.
6. The system should handle customer information, including name, contact details, and driver's license information.
7. The system should handle payment processing for reservations.
8. The system should be able to handle concurrent reservations and ensure data consistency.

## Design

```mermaid
classDiagram
    class Car {
        - CarTypeEnum Type
        - string Model
        - short Year
        - string PlateNumber
        - decimal RentPricePerDay
        - List<Request> Requests
        + bool IsAvailable()
        + bool Reserver(Request)
        + void Cancel()
    }
    class CarTypeEnum {
        <<enumeration>>
    }
    class Customer {
        + string Name 
        + string PhoneNumber
        + string LicenseInformation
        + Request[] Requests
        + void AddRequest(Request)
        + void CancelRequest(Request)
        + void Pay(Request, IPayment)
    }
    class Request {
        + Car Car
        + Customer Customer
        + DateTime RequestDate
        + DateTime ReturnDate
        + bool IsCanceled
    }
    class ISearch {
        + Car[] Search(Filter)
    }
    ISearch <-- SearchService
    class Filter {
        + CarTypeEnum? Type
        + decimal? MinPrice
        + decimal? MaxPrice
        + bool? IsAvailable 
    }
    ISearch --> Filter 
    class IPayment {
        + bool Pay(Customer, Amount)
    }
    IPayment <-- cashPayment 
    class IReserve {
        + void AddRequest(Request)
        + void CancelRequest(Request)
    }
    IReserve <-- ReserveService 
    class CarRentalSystem {
        + Car[] Cars
        + Customer[] Customers
        + Request[] Requests
        + ISearch SearchService
        + IReserve ReserveService
    }
    Request "*" -- "1" Customer
    Request  -- "1" Car
    IPayment <-- Customer
    CarRentalSystem --> IReserve
    CarRentalSystem --> ISearch
    CarRentalSystem "1" -- "*" Car
    CarRentalSystem "1" -- "*" Customer
    CarRentalSystem "1" -- "*" Request
```
