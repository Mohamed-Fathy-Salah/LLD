using InstallmentPlanner.Enums;

namespace InstallmentPlanner.Models;

public class InstallmentGroupSpecs(decimal margin, decimal cbeRate, int numberOfInstallments, DateOnly firstPaymentDate, HashSet<int> servingInterestPeriods, HashSet<int> fullCapitalizationPeriods, Dictionary<int, decimal> periodsStepPercentage, RepaymentStructureEnum repaymentStructure)
{
    public decimal Margin { get; } = margin;
    public decimal CbeRate { get; } = cbeRate;
    public int NumberOfInstallments { get; } = numberOfInstallments;
    public DateOnly FirstPaymentDate { get; } = firstPaymentDate;
    public HashSet<int> ServingInterestPeriods { get; } = servingInterestPeriods;
    public HashSet<int> FullCapitalizationPeriods { get; } = fullCapitalizationPeriods;
    public Dictionary<int, decimal> PeriodsStepPercentage { get; } = periodsStepPercentage;
    public RepaymentStructureEnum RepaymentStructure { get; } = repaymentStructure;

    public int GetNumberOfMonths() => RepaymentStructure switch
    {
        RepaymentStructureEnum.Monthly => 1,
        RepaymentStructureEnum.Quarterly => 4,
        RepaymentStructureEnum.SemiAnnual => 6,
        RepaymentStructureEnum.Annual => 12,
        _ => throw new Exception()
    };
}
