namespace InstallmentPlanner.Models;

public class Accrual(int period, decimal opening, decimal principalAmount, decimal interestAmount, decimal revenue, decimal closing, DateOnly date)
{
    public int Period { get; } = period;
    public decimal Opening { get; } = opening;
    public decimal PrincipalAmount { get; } = principalAmount;
    public decimal InterestAmount { get; } = interestAmount;
    public decimal Revenue { get; } = revenue;
    public decimal Closing { get; } = closing;
    public DateOnly Date { get; } = date;
}
