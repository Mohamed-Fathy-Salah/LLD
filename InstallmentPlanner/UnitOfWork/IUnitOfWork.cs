namespace InstallmentPlanner.UnitOfWork;

public interface IUnitOfWork
{
    public List<Models.Accrual> accrual { get; init; }
    public List<Models.Action> action { get; init; }
    public List<Models.CalculationSheet> calculationSheet { get; init; }
    public List<Models.Company> company { get; init; }
    public List<Models.Installment> installment { get; init; }
    public List<Models.InstallmentDetail> installmentDetail { get; init; }
    public List<Models.InstallmentGroupSpecs> installmentGroupSpecs { get; init; }
}
