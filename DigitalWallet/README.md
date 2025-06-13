# Designing a Digital Wallet Service

## Requirements
1. The digital wallet should allow users to create an account and manage their personal information.
1. Users should be able to add and remove payment methods, such as credit cards or bank accounts.
1. The digital wallet should support fund transfers between users and to external accounts.
1. The system should handle transaction history and provide a statement of transactions.
1. The digital wallet should support multiple currencies and perform currency conversions.
1. The system should ensure the security of user information and transactions.
1. The digital wallet should handle concurrent transactions and ensure data consistency.
1. The system should be scalable to handle a large number of users and transactions.

## Design
```mermaid
classDiagram
    class User {
        + string Name
        + string Email
        - string _password
        + bool IsCorrectPassword(string password)
        + Payment[] Payments
        - string _hashPassword(string password)
    }
    class IObserver {
        + void InCommingTransaction(Transaction)
    }
    IObserver <-- User

    class Payment {
        <<abstract>>
        + int Id
        + User Owner
        + decimal Amount
        + CurrencyEnum Currency
        + void AddBalance(decimal Amount)
        + bool TryTakeBalance(decimal Amount)
    }
    Payment <-- CreditCardPayment
    Payment <-- BankAccountPayment
    class CurrencyEnum {
        <<enumeration>>
        EGP
        USD
        EUR
    }
    class CurrencyExchager {
        <<singleton>>
        - ConcurrentDictionary~CurrencyEnum,[CurrencyEnum, decimal]~ _exchageRates
        + void SetCurrencyExchage(CurrencyEnum from, CurrencyEnum to, decimal rate)
        + decimal Convert(CurrencyEnum from, CurrencyEnum to, decimal amount)
    }
    class Transaction {
        + Payment Sender
        + Payment Receiver
        + decimal Amount
        + CurrencyEnum Currency
        - bool _isSuccessfull
        + DateTime CreatedAt
        + void MarkDone() notify receiver
    }
    class AuthService {
        + User[] users
        + User SignIn(string email, string password)
        + User SignUp(string name, string email, string password)
    }
    class DigitalWallet {
        <<singleton>>
        + AuthService AuthService
        - Transaction[] _transactions
        + Transaction[] GetTransaction(User)
        + bool Transfer(Payment sender, Payment receiver, decimal amount, CurrencyEnum currency)
    }
    DigitalWallet --> AuthService
    AuthService "1" -- "*" User
    User "1" -- "*" Payment
    DigitalWallet "1" -- "*" Transaction
```
