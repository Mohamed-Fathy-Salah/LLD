namespace InstallmentPlanner.Models;

public class Company(int companyId, decimal shareAmount, decimal sharePercentage, bool isMainCollector, bool shouldCreateJournals, int[] includedPeriods)
{
    public int CompanyId { get; } = companyId;
    public decimal ShareAmount { get; } = shareAmount;
    public decimal SharePercentage { get; } = sharePercentage;
    public bool IsMainCollector { get; } = isMainCollector;
    public bool ShouldCreateJournals { get; } = shouldCreateJournals;
    public int[] IncludedPeriods { get; } = includedPeriods;
}