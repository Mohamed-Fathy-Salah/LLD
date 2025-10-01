namespace InstallmentPlanner;

public class InstallmentGroupSpecs
{
    public int numberOfInstallments { get; set; }
    public GracePeriodEnum gracePeriod { get; set; }
    public decimal stepPercent { get; set; }
    public decimal interestRate { get; set; }
}
