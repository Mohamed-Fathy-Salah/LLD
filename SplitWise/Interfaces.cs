namespace SplitWise;

public interface IGroupObserver
{
    internal void AddedToGroup(Group group);
    internal void AddedToExpense(Group group, Expense expense);
    internal void GotMoney(Group group, Expense expense, IGroupObserver participant);
    internal void PaidMoney(Group group, Expense expense);
}
