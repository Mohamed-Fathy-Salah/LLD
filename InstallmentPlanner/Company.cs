namespace InstallmentPlanner;

public class Company
{
    public CompanySpecs specs { get; set; }
    public int companyId { get; set; }
}

public class CompanySpecs
{
    public decimal shareAmount { get; set; }
    public decimal sharePercentage { get; set; }
}
