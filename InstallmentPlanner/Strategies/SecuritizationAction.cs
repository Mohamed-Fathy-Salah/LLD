using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class SecuritizationAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Securitization;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement securitization logic
        throw new NotImplementedException();
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement securitization rollback logic
        throw new NotImplementedException();
    }
}
