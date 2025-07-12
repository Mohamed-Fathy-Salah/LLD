# Designing Splitwise

## Requirements
1. The system should allow users to create accounts and manage their profile information.
2. Users should be able to create groups and add other users to the groups.
3. Users should be able to add expenses within a group, specifying the amount, description, and participants.
4. The system should automatically split the expenses among the participants based on their share.
5. Users should be able to view their individual balances with other users and settle up the balances.
6. The system should support different split methods, such as equal split, percentage split, and exact amounts.
7. Users should be able to view their transaction history and group expenses.
8. The system should handle concurrent transactions and ensure data consistency.

## Design
```mermaid
classDiagram
    class User {
        + string name
        + Dictionary~Group,Expense[]~ GroupExpenses
        + Dictionary~User, (Group[], decimal paid, decimal owes)~ UserBalances
        + Group CreateGroup(string name, Users[] participants)
        + void SettleBalances(User)
    }
    class IGroupObserver {
        + void AddedToGroup(Group)
        + void AddedToExpense(Group, Expense)
        + void Settled(Expense)
    }
    class Group {
        + string name
        + Expense[] expenses
        + Dictionary~IGroupObserver,(decimal paid, decimal owes)~ participantsPaymentDetails
        + void AddParticipant(IGroupObserver)
        + void AddExpense(description, payer, amount, Dictionary~IGroupObserver,decimal~ userShares, SplitStrategyEnum)
        + void Settle(IGroupObserver, Expense)
    }
    class Expense {
        + string description
        + IGroupObserver payer
        + decimal amount
        + Dictionary~IGroupObserver,decimal~ userShareAmounts
        + SplitStrategy split
        + DateTime CreatedAt
    }
    class SplitStrategyEnum {
        <<enumeration>>
        EQUAL
        PERCENTAGE
        AMOUNT
    }
    class SplitStrategyFactory {
        <<singleton>>
        - Lazy<EqualSplit> equalSplit
        - Lazy<PercentageSplit> percentageSplit
        - Lazy<ExactAmountSplit> exactAmountSplit
        + SplitStrategy GetSplitStrategy(SplitStrategyEnum)
    }
    class SplitStrategy {
        <<abstract>>
        + Dictionary~IGroupObserver,decimal~ UserShareAmounts(userShares, amount)
    }
    SplitStrategy <-- EqualSplit
    SplitStrategy <-- PercentageSplit
    SplitStrategy <-- ExactAmountSplit
    Group "1" -- "*" IGroupObserver
    IGroupObserver <-- User
    Group "1" -- "*" Expense
    Expense --> SplitStrategy
```
