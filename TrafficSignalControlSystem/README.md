# Designing a Traffic Signal Control System

## Requirements
1. The traffic signal system should control the flow of traffic at an intersection with multiple roads.
2. The system should support different types of signals, such as red, yellow, and green.
3. The duration of each signal should be configurable and adjustable based on traffic conditions.
4. The system should handle the transition between signals smoothly, ensuring safe and efficient traffic flow.
5. The system should be able to detect and handle emergency situations, such as an ambulance or fire truck approaching the intersection.
6. The system should be scalable and extensible to support additional features and functionality.

```mermaid
classDiagram
    class Intersection {
        +int id
        +List~Road~ roads
    }

    class Road {
        +int id
        +string name
        +int sensorId
        +SignalColor signal
    }
    class TrafficSignalController {
        -Dictionary~Intersection, SignalPhaseSequence~ schedules
        -Dictionary~int, Road~ roadSensors
        +configurePhase(Intersection, SignalPhaseSequence)
        +start()
        +stop()
        +void onVehicleArrived(Road)
        +void onVehicleDepartd(Road)
    }

    class SignalPhaseSequence {
        +Dictionary~SignalColor,TimeSpan~ phases
        -int currentIndex
        +SignalColor nextPhase()
        +reset()
    }

    class SignalColor {
        byte red
        byte green
        byte blue
    }

    class EmergencyVehicleDetector {
        +List~EmergencyVehicleObserver~ observers
        +void detect(int sensorId)
    }

    class EmergencyVehicleObserver {
        <<interface>>
        +void onVehicleArrived(Road)
        +void onVehicleDepartd(Road)
    }

    class SignalFactory {
        SignalColor Get~T~ where T:SignalColor
    }

    TrafficSignalController "1" -- "*" Intersection
    Intersection "1" -- "*" Road
    TrafficSignalController o-- SignalPhaseSequence
    TrafficSignalController ..> EmergencyVehicleObserver
    EmergencyVehicleObserver "*" -- "1" EmergencyVehicleDetector 
    RedSignal ..> SignalColor
    GreenSignal ..> SignalColor
    YellowSignal ..> SignalColor
    SignalColor <-- SignalFactory 
```  
