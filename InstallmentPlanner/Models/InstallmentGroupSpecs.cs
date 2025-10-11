using InstallmentPlanner.Enums;

namespace InstallmentPlanner.Models;

public class InstallmentGroupSpecs(decimal margin, int numberOfInstallments, int[] servingInterestPeriods, int[] fullCapitalizationPeriods, Dictionary<int, decimal> periodsStepPercentage, RepaymentStructureEnum repaymentStructure)
{
    public decimal Margin { get; } = margin;
    public int NumberOfInstallments { get; } = numberOfInstallments;
    public int[] ServingInterestPeriods { get; } = servingInterestPeriods;
    public int[] FullCapitalizationPeriods { get; } = fullCapitalizationPeriods;
    public Dictionary<int, decimal> PeriodsStepPercentage { get; } = periodsStepPercentage;
    public RepaymentStructureEnum RepaymentStructure { get; } = repaymentStructure;
}