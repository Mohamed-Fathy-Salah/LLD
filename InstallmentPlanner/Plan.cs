namespace InstallmentPlanner;

public class Plan
{
    public RepaymentScheduleEnum repaymentSchedule { get; set; }
    public int contractId { get; set; }
    public Company[] companies { get; set; }
    public InstallmentGroupSpecs[] installmentGroupsSpecs { get; set; }
    public PlanAction[] actions { get; set; }

    public async Task RunAsync(PlanAction action)
    {
        action.date = DateTime.UtcNow;
        await action.RunAsync(this);
    }
}
