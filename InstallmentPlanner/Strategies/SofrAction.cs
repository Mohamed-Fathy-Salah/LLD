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
        throw new NotImplementedException();
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement SOFR rollback logic
        throw new NotImplementedException();
    }
}
