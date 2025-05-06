# Designing a Parking Lot System

## Requirements
1. The parking lot should have multiple levels, each level with a certain number of parking spots.
2. The parking lot should support different types of vehicles, such as cars, motorcycles, and trucks.
3. Each parking spot should be able to accommodate a specific type of vehicle.
4. The system should assign a parking spot to a vehicle upon entry and release it when the vehicle exits.
5. The system should track the availability of parking spots and provide real-time information to customers.
6. The system should handle multiple entry and exit points and support concurrent access.

## Classes, Interfaces and Enumerations

```mermaid
classDiagram
  %% Vehicles & Types
  class Vehicle {
    - String plateNumber
    - VehicleTypeEnum type
  }
  class VehicleTypeEnum {
    <<enumeration>>
    CAR
    MOTORCYCLE
    TRUCK
  }

  %% Allocation Strategy
  class ISpotAllocationStrategy {
    <<interface>>
    +Spot chooseSpot(Level lvl, Vehicle v)*
  }
  class NearestFirstStrategy {
    +Spot chooseSpot(Level lvl, Vehicle v)
  }
  class ZonedStrategy {
    +Spot chooseSpot(Level lvl, Vehicle v)
  }

  class ExitGate {
    + bool validateAndProcess(Ticket t, IPayment p)
  }

  class IPayment {
      <<interface>>
    + bool processPayment(Ticket t)*
  }
  class CashPayment {
    + bool processPayment(Ticket t)
  }
  class CreditCardPayment {
    + bool processPayment(Ticket t)
  }

  ExitGate --> IPayment
  CashPayment ..|> IPayment
  CreditCardPayment ..|> IPayment
  ISpotAllocationStrategy <|.. NearestFirstStrategy
  ISpotAllocationStrategy <|.. ZonedStrategy
```

```mermaid
classDiagram
  %% Core Domain
  class ParkingLot {
    - Level[] levels
    - Map~String,Ticket~ activeTickets
    + bool hasEmptySpots(VehicleTypeEnum type)
    + Ticket parkVehicle(Vehicle v)
    + bool exitVehicle(Ticket t)
  }

  class Level {
    - int levelNumber
    - Map[VehicleTypeEnum, [int,Spot[]]] spotsByType int->count of free spots
    - ILevelObserver[] observers
    + bool hasEmptySpots(VehicleTypeEnum type)
    + Spot allocateSpot(Vehicle v)
    + bool freeSpot(Spot s)
    + void addObserver(Observer o)
    + void removeObserver(Observer o)
    + void notifyObservers()
  }

  class Spot {
    <<abstract>>
    -int x
    -int y
    -Vehicle? vehicle
    +bool isEmpty()
    +bool assign(Vehicle v)
    +bool free()
    +bool CanFitVehicle(VehicleTypeEnum vehicleType)*
  }
  class CarSpot {
    +bool CanFitVehicle(VehicleTypeEnum vehicleType)
  }
  class MotorcycleSpot {
    +bool CanFitVehicle(VehicleTypeEnum vehicleType)
  }
  class TruckSpot {
    +bool CanFitVehicle(VehicleTypeEnum vehicleType)
  }

  %% Entry / Exit & Ticketing
  class EntryGate {
    + Ticket issueTicket(Vehicle v, ISpotAllocationStrategy s)
  }
  class Ticket {
    + GUID id
    + DateTime entryTime
    + Level level
    + Spot spot
  }

  %% Observer Pattern
  class ILevelObserver {
    <<interface>>
    +void update(Level lvl)
  }
  class DisplayBoard {
    +void update(Level lvl)
  }
  class MobileApp {
    +void update(Level lvl)
  }

  %% Relationships
  ParkingLot "1" o-- "*" Level
  ParkingLot --> Ticket 
  Level "1" o-- "*" Spot
  Level ..> ILevelObserver : notify
  Spot <|-- CarSpot
  Spot <|-- MotorcycleSpot
  Spot <|-- TruckSpot
  EntryGate --> ParkingLot
  ILevelObserver <|.. DisplayBoard
  ILevelObserver <|.. MobileApp
```

## Entry Gate Sequence Diagram
``` mermaid
sequenceDiagram
    participant V as Vehicle
    participant E as EntryGate
    participant L as ParkingLot
    participant Le as Level
    participant S as Spot
    participant T as Ticket
    participant DB as DisplayBoard

    V->>E: IssueTicket(vehicle, allocationStrategy)
    alt HasEmptySpots(vehicleType)
        E->>L: ParkVehicle(vehicle, allocationStrategy)
    end
    loop for each level in ParkingLot
        alt HasEmptySpots(vehicle)
            L->>Le: AllocateSpot(vehicle, allocationStrategy)
        end
    end
    Le->>S: Assign(vehicle)
    Le->>DB: NotifyAll()
    Le-->>L: SpotAssigned(x,y)
    L->>E: ReturnTicket(T)
    Note over DB: UI updates in real time
```

## Exit Gate Sequence Diagram
```mermaid
sequenceDiagram
    participant V as Vehicle
    participant EG as ExitGate
    participant PL as ParkingLot
    participant L as Level
    participant S as Spot
    participant T as Ticket
    participant P as Payment

    V->>EG: RequestExit(ticket)
    EG->>PL: ValidateTicket(ticket)
    alt ValidTicket
        PL->>L: FindSpotByTicket(ticket)
        L->>S: FreeSpot(ticket.spot)
        S->>L: SpotFreed
        PL->>EG: ProcessExit(ticket)
        EG->>P: ProcessPayment(ticket)
        P->>EG: PaymentProcessed
        EG->>V: ExitConfirmed
    else InvalidTicket
        EG->>V: InvalidTicketError
    end
```
