using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Strategies;

public class OffloadingAction : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Offloading;

    public Models.Action Run(BaseActionDto dto)
    {
        // TODO: Implement offloading logic
        throw new NotImplementedException();
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement offloading rollback logic
        throw new NotImplementedException();
    }
}
