using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class EuriborAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Euribor;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement Euribor logic
        throw new NotImplementedException();
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement Euribor rollback logic
        throw new NotImplementedException();
    }
}
