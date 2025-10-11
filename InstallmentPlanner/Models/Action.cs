using InstallmentPlanner.Enums;

namespace InstallmentPlanner.Models;

public class Action(InstallmentGroupSpecs installmentGroupSpecs, Company[] companies, DateTime createdAt, ActionTypeEnum type)
{
    public List<Installment> Installments { get; set; } = [];
    public InstallmentGroupSpecs InstallmentGroupSpecs { get; } = installmentGroupSpecs;
    public Company[] Companies { get; } = companies;
    public DateTime CreatedAt { get; } = createdAt;
    public ActionTypeEnum Type { get; } = type;
}
