using InstallmentPlanner.Models;

namespace InstallmentPlanner.Services;

public interface ICalculationService
{
    public void MakePlan(Models.Action action);
    public void GeneratePlanSkeleton(Models.Action action);
    public void GoalSeek(Models.Action action);
    public void Break(Installment installment);
    public void GenerateAccrual(Installment installment);
    public void GenerateAccrual(InstallmentDetail installment);
}
