namespace InstallmentPlanner;

public class Installment
{
    public ClubInstallment[] clubInstallments { get; set; }
    public InstallmentSpecs specs { get; set; }
    public void Aggregate(Plan plan)
    {
        //todo;
    }
}

public class ClubInstallment
{
    public int companyId { get; set; }
    public InstallmentSpecs specs { get; set; }
}

public class InstallmentSpecs
{
    public int period { get; set; }
    public decimal principal { get; set; }
    public decimal interestRate { get; set; }
    public decimal interestAmount { get; set; }
    public decimal rent { get; set; }
    public decimal stepPercent { get; set; }
    public DateTime date { get; set; }
    public GracePeriodEnum gracePeriod { get; set; }
}
