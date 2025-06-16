# Designing an Airline Management System

## Requirements
1. The airline management system should allow users to search for flights based on source, destination, and date.
2. Users should be able to book flights, select seats, and make payments.
3. The system should manage flight schedules, aircraft assignments, and crew assignments.
4. The system should handle passenger information, including personal details and baggage information.
5. The system should support different types of users, such as passengers, airline staff, and administrators.
6. The system should be able to handle cancellations, refunds, and flight changes.
7. The system should ensure data consistency and handle concurrent access to shared resources.
8. The system should be scalable and extensible to accommodate future enhancements and new features.

```mermaid
classDiagram
    class User {
        <<abstract>>
        + string Name
        + Buggage[] Buggages 
        + void FlightChanged(Flight)
    }
    class Buggage {
        + int id
        + string Details
    }
    class ISearchService {
        + Flight[] Search(FlightsFilter)
    }
    class SearchService {
        + FlightRepository FlightRepository
    }
    class FlightRepository {
        + Flight[] Flights
    }
    class FlightsFilter {
        + DestinationsEnum Source
        + DestinationsEnum Destination
        + DateTime Date
    }
    class AirLineSystem {
        <<singleton>>
        + ISearchService SearchService
        + User[] Users
        + FlightRepository FlightRepository
        + bool BookFlight(User, Flight, Seat[], IPayment)
        + bool CancelFlight(User, Flight, IPayment)
        + void UpdateFlight(Flight, newFrom, newTo, newSource, newDestination)
    }
    class Flight {
        + AirCraft AirCraft
        + DateTime From
        + DateTime To
        + DestinationsEnum Source
        + DestinationsEnum Destination
        + decimal PricePerSeat
        - ConcurrentDictionary~Seat,User~ _bookedSeats
        + bool Book(User, Seat[])
        + void NotifyAll()
        + Seat[] GetEmptySeats()
        + void Update(newAircraft, newPricePerSeat, newFrom, newTo, newSource, newDestination)
    }
    class DestinationsEnum {
        <<enumeration>>
    }
    class AirCraft {
        + int Id
        + Seat[] Seats
    }
    class Seat {
        + int Id
    }
    class IPayment {
        <<interface>>
        + bool Pay(User, amount)
    }
    User --> "0..*" Buggage : owns
    AirLineSystem --> "0..*" User : registers
    AirLineSystem --> ISearchService : uses
    AirLineSystem --> FlightRepository : manages
    AirLineSystem --> Flight : books/makes/cancels/changes
    AirLineSystem --> IPayment : uses

    SearchService --> FlightRepository : uses

    ISearchService <|.. SearchService : implements

    FlightRepository --> "0..*" Flight : stores
    Flight --> "0..*" Seat : contains (via AirCraft)
    Flight --> DestinationsEnum : has source/destination
    Flight --> "0..*" User : bookedBy (via _bookedSeats)

    AirCraft --> "0..*" Seat : contains
    Flight --> "0..1" AirCraft : uses

    Flight --> User : notifies
    User --> Flight : subscribed to (for notification)

    AirLineSystem --> FlightsFilter : uses (in Search)

    IPayment <|.. SomeConcretePaymentClass : implements
```
