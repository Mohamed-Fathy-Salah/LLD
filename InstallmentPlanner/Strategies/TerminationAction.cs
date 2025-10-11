using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class TerminationAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Termination;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement termination logic
        throw new NotImplementedException();
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement termination rollback logic
        throw new NotImplementedException();
    }
}
