# Designing a Vending Machine

## Requirements
1. The vending machine should support multiple products with different prices and quantities.
1. The machine should accept coins and notes of different denominations.
1. The machine should dispense the selected product and return change if necessary.
1. The machine should keep track of the available products and their quantities.
1. The machine should provide an interface for restocking products and collecting money.
1. The machine should handle exceptional scenarios, such as insufficient funds or out-of-stock products.

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
        addNewProduct(string name, int priceInCents)
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
        addNewProduct(string name, int priceInCents)
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
        addNewProduct(string name, int priceInCents)
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
    DispensingState ..> VendingMachineState
    ReturningChangeState ..> VendingMachineState
```

```mermaid
stateDiagram-v2
    [*] --> Idle
    Idle --> Open : openMachine
    Open --> Idle : done
    Open --> Open : addProduct
    Open --> Open : collectMoney
    Idle --> AcceptingProducts : start
    AcceptingProducts --> AcceptingProducts : [add,addNew,remove]Product
    AcceptingProducts --> AcceptingMoney : done
    AcceptingProducts --> Idle : cancel
    AcceptingMoney --> AcceptingMoney : insertMoney
    AcceptingMoney --> Idle : cancel
    AcceptingMoney --> Dispensing : [insertMoney >= price]
    Dispensing --> ReturningChange : done
    ReturningChange --> Idle : done
```
