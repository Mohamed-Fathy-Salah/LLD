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
        throw new NotImplementedException();
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement corridor rollback logic
        throw new NotImplementedException();
    }
}
