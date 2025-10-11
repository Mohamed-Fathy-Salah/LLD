namespace InstallmentPlanner.Models;

public class Installment(int period, decimal opening, decimal principalAmount, decimal interestAmount, decimal rent, decimal closing, DateOnly date, decimal interestRate, InstallmentDetail[] installmentDetails, Accrual[] accruals)
{
    public int Period { get; } = period;
    public decimal Opening { get; } = opening;
    public decimal PrincipalAmount { get; } = principalAmount;
    public decimal InterestAmount { get; } = interestAmount;
    public decimal Rent { get; } = rent;
    public decimal Closing { get; } = closing;
    public DateOnly Date { get; } = date;
    public decimal InterestRate { get; } = interestRate;
    public InstallmentDetail[] InstallmentDetails { get; } = installmentDetails;
    public Accrual[] Accruals { get; } = accruals;
}