# Designing Restaurant Management System

## Requirements
1. The restaurant management system should allow customers to place orders, view the menu, and make reservations.
1. The system should manage the restaurant's inventory, including ingredients and menu items.
1. The system should handle order processing, including order preparation, billing, and payment.
1. The system should support multiple payment methods, such as cash, credit card, and mobile payments.
1. The system should manage staff information, including roles, schedules, and performance tracking.
1. The system should generate reports and analytics for management, such as sales reports and inventory analysis.
1. The system should handle concurrent access and ensure data consistency.

## Design
```mermaid
classDiagram
    class Customer {
        + string Name
    }
    class Order {
        + Customer customer
        + Dictionary~Item, int~ ItemsQuantities
        + DateTime CreatedAt
        + OrderStatus Status
        + decimal Price
    }
    class OrderStatus {
        <<enumeration>> 
        Pending
        Preparing
        Billing
        Paid
    }
    class Inventory {
        - Dictionary~Ingredient,quantity~ _ingredientsQuantities
        - Item[] _items
    }
    class IInventory {
        <<interface>>
        + Ingredient[] GetIngredients()
        + void AddIngredient(Ingredient, quantity)
        + bool TryTakeIngredient(Ingredient, quantity)
        + Item[] GetMenuItems()
        + bool TryTakeMenuItem(Item, quantity)
        + void AddMenuItem(Item)
    }
    Inventory "1" -- "*" Item
    Inventory "1" -- "*" Ingredient
    class Ingredient {
        + int Id
        + string Name
        + decimal PricePerUnit
    }
    class Item {
        + int Id
        - Dictionary~Ingredient, float~ _ingredientsQuantities
        + string Name
        + decimal Price
        + float Multiplier -- from 0 to 1
    }
    IInventory <-- Inventory
    class IPayment {
        + bool Pay(Customer, amount)
    }
    IPayment <-- CashPayment
    IPayment <-- CreditCardPayment
    IPayment <-- MobilePayment
    class IOrderService {
        <<interface>>
        + Order? PlaceOrder(Staff, Customer, Dictionary~Item,int~ itemsQuantities)
        + void PrepareOrder(Staff, Order)
        + bool TryPayOrder(Staff, Order, IPayment, amount)
        + Order[] GetOrdersByStatusAfterDate(OrderStatus, DateTime)
    }
    class OrderService {
        - Order[] _orders
        - IInventory _inventory
    }
    IOrderService <-- OrderService
    OrderService "1" -- "*" Order
    IOrderService --> IPayment
    class Staff {
        + string Name
        + RoleEnum Role
    }
    class RoleEnum {
        <<enumeration>>
        Cheif
        Cashier
    }
    class IStaffPerformanceService {
        + void StaffHandledOrder(Staff, Order)
        + int[] GetStaffPerformance(Staff, int lastXDays)
    }
    class StaffPerformanceService {
        - Dictionary~Staff, Order[]~ orders
    }
    IStaffPerformanceService <-- StaffPerformanceService
    class IStaffRepository {
        <<interface>>
        + Staff[] GetStaffs()
        + Staff AddStaff(name, role)
        + void RemoveStaff(Staff)
    }
    class StaffRepository {
        - Staff[] _staffs
    }
    IStaffRepository <-- StaffRepository
    class ReportsGenerator {
        - IInventory _inventory
        - IOrderService _orderService
        + Ingredient[] GetEmptyIngredients()
        + Dictionary~DateOnly, decimal~ GetSales(int lastXDays)
    }
    ReportsGenerator --> IInventory
    ReportsGenerator --> IOrderService
```
