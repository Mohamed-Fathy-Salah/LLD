using InstallmentPlanner.Enums;

namespace InstallmentPlanner.Models;

public class Action(Installment[] installments, InstallmentGroupSpecs[] installmentGroupSpecs, Company[] companies, DateTime createdAt, ActionTypeEnum type)
{
    public Installment[] Installments { get; } = installments;
    public InstallmentGroupSpecs[] InstallmentGroupSpecs { get; } = installmentGroupSpecs;
    public Company[] Companies { get; } = companies;
    public DateTime CreatedAt { get; } = createdAt;
    public ActionTypeEnum Type { get; } = type;
}
