using InstallmentPlanner.Enums;

namespace InstallmentPlanner.Models;

public class CalculationSheet(Models.Action[] actions, int contractId, ContractStatusEnum status, PaymentTypeEnum paymentType)
{
    public Models.Action[] Actions { get; } = actions;
    public int ContractId { get; } = contractId;
    public ContractStatusEnum Status { get; } = status;
    public PaymentTypeEnum PaymentType { get; } = paymentType;
}