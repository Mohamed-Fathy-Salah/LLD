using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class InitiationAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Initiation;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement initiation logic
        return new Models.Action(
            installments: Array.Empty<Installment>(),
            installmentGroupSpecs: Array.Empty<InstallmentGroupSpecs>(),
            companies: Array.Empty<Company>(),
            createdAt: DateTime.UtcNow,
            type: Type);
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement initiation rollback logic
        return new Models.Action(
            installments: Array.Empty<Installment>(),
            installmentGroupSpecs: Array.Empty<InstallmentGroupSpecs>(),
            companies: Array.Empty<Company>(),
            createdAt: DateTime.UtcNow,
            type: Type);
    }
}
