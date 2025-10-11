using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;

namespace InstallmentPlanner.Strategies;

public interface IActionStrategy
{
    ActionTypeEnum Type { get; }
    Models.Action Run(BaseActionDto dto);
    Models.Action Rollback(BaseActionDto dto);
}
