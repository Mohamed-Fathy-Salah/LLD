# Designing a Coffee Vending Machine

## Requirements
1. The coffee vending machine should support different types of coffee, such as espresso, cappuccino, and latte.
1. Each type of coffee should have a specific price and recipe (ingredients and their quantities).
1. The machine should have a menu to display the available coffee options and their prices.
1. Users should be able to select a coffee type and make a payment.
1. The machine should dispense the selected coffee and provide change if necessary.
1. The machine should track the inventory of ingredients and notify when they are running low.
1. The machine should handle multiple user requests concurrently and ensure thread safety.

```mermaid
stateDiagram-v2
    [*] --> Idle
    Idle --> Open : openMachine
    Open --> Idle : done
    Open --> Open : [add,addNew]Ingredient
    Open --> Open : collectMoney
    Idle --> AcceptingRequests : start
    AcceptingRequests --> AcceptingRequests : [add,remove]Request
    AcceptingRequests --> AcceptingMoney : done
    AcceptingRequests --> Idle : cancel
    AcceptingMoney --> AcceptingMoney : insertMoney
    AcceptingMoney --> ReturningChange : cancel
    AcceptingMoney --> Preparing : [insertMoney >= price]
    Preparing --> Dispensing : done
    Dispensing --> ReturningChange : done
    ReturningChange --> Idle : done
```

```mermaid
classDiagram
    class Product {
        int id
        string name
        int quantity
        int priceInCents
    }

    class Repository {
        Map~int,Product~ products
        bool isAvailable(int productId)
        void addNewProduct(string name, int priceInCents)
        void addProduct(int productId, int quantity)
        void dispenseProduct(int productId, int quantity)
    }

    class VendingMachine {
        int amountInCents
        Repository repo
        VendingMachineState currentState
        start()
        done()
        cancel()
        openMachine()
        collectMoney(int amount)
        addNewIngredient(string name, int priceInCents)
        addIngredient(int ingredientId, int quantity)
        addProduct(int productId, int quantity)
        removeProduct(int productId)
        insertMoney(int amount)
    }

    class VendingMachineState {
        <<abstract>>
        IVendingMachineState context
        start()
        done()
        cancel()
        openMachine()
        collectMoney(int amount)
        addNewIngredient(string name, int priceInCents)
        addIngredient(int ingredientId, int quantity)
        addProduct(int productId, int quantity)
        removeProduct(int productId)
        insertMoney(int amount)
    }

    class IVendingMachineState {
        start()
        done()
        cancel()
        openMachine()
        collectMoney(int amount)
        addNewIngredient(string name, int priceInCents)
        addIngredient(int ingredientId, int quantity)
        addProduct(int productId, int quantity)
        removeProduct(int productId)
        insertMoney(int amount)
    }

    class IdleState {
        openMachine()
        start()
    }

    class OpenState {
        done()
        addProduct(int productId, int quantity)
        collectMoney(int amount)
    }

    class AcceptingProductsState {
        addProduct(int productId, int quantity)
        done()
        cancel()
    }

    class AcceptingMoneyState {
        insertMoney(int amount)
        cancel()
    }

    class PreparingState {
        done()
    }

    class DispensingState {
        done()
    }

    class ReturningChangeState {
        done()
    }

    VendingMachineState ..> IVendingMachineState
    VendingMachine ..> IVendingMachineState
    VendingMachine --> Repository
    Repository --> Product
    IdleState ..> VendingMachineState
    OpenState ..> VendingMachineState
    AcceptingProductsState ..> VendingMachineState
    AcceptingMoneyState ..> VendingMachineState
    PreparingState ..> VendingMachineState
    DispensingState ..> VendingMachineState
    ReturningChangeState ..> VendingMachineState
```
