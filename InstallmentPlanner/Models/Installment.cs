namespace InstallmentPlanner.Models;

public class Installment(int period, DateOnly date, int numberOfDays, decimal interestRate)
{
    public int Period { get; } = period;
    public decimal Opening { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal Rent { get; set; }
    public decimal Closing { get; set; }
    public DateOnly Date { get; } = date;
    public int NumberOfDays { get; } = numberOfDays;
    public decimal InterestRate { get; } = interestRate;
    public List<InstallmentDetail> InstallmentDetails { get; set; } = default!;
    public List<Accrual> Accruals { get; set; } = default!;

    public override string ToString() => $"{Period} {Opening} {Date} {NumberOfDays} {Rent} {PrincipalAmount} {InterestAmount} {Closing} {InterestRate}";
}
