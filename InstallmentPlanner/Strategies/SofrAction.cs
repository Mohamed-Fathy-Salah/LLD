using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class SofrAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Sofr;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement SOFR logic
        return new Models.Action(
            installments: Array.Empty<Installment>(),
            installmentGroupSpecs: Array.Empty<InstallmentGroupSpecs>(),
            companies: Array.Empty<Company>(),
            createdAt: DateTime.UtcNow,
            type: Type);
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement SOFR rollback logic
        return new Models.Action(
            installments: Array.Empty<Installment>(),
            installmentGroupSpecs: Array.Empty<InstallmentGroupSpecs>(),
            companies: Array.Empty<Company>(),
            createdAt: DateTime.UtcNow,
            type: Type);
    }
}
