using InstallmentPlanner.Models;

namespace InstallmentPlanner.Services;

public interface ICalculationService
{
    public Installment[] GoalSeek(Models.Action action);
    public InstallmentDetail[] Break(Installment installment);
    public Accrual[] GenerateAccrual(Installment installment);
    public Accrual[] GenerateAccrual(InstallmentDetail installment);
}
