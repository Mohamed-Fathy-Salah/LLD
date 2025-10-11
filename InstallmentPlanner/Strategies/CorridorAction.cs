using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class CorridorAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Corridor;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement corridor logic
        return new Models.Action(
            installments: Array.Empty<Installment>(),
            installmentGroupSpecs: Array.Empty<InstallmentGroupSpecs>(),
            companies: Array.Empty<Company>(),
            createdAt: DateTime.UtcNow,
            type: Type);
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement corridor rollback logic
        return new Models.Action(
            installments: Array.Empty<Installment>(),
            installmentGroupSpecs: Array.Empty<InstallmentGroupSpecs>(),
            companies: Array.Empty<Company>(),
            createdAt: DateTime.UtcNow,
            type: Type);
    }
}
