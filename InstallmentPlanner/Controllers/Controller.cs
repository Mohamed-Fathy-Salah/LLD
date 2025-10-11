using InstallmentPlanner.DTOs;
using InstallmentPlanner.Factories;

namespace InstallmentPlanner.Controllers;

public class Controller(ActionFactory actionFactory)
{
    public Models.Action Initiate(InitiationDto dto)
    {
        return actionFactory.Run(dto);
    }

    public Models.Action Corridor(CorridorDto dto)
    {
        return actionFactory.Run(dto);
    }

    public Models.Action Sofr(SofrDto dto)
    {
        return actionFactory.Run(dto);
    }

    public Models.Action Euribor(EuriborDto dto)
    {
        return actionFactory.Run(dto);
    }

    public Models.Action Termination(TerminationDto dto)
    {
        return actionFactory.Run(dto);
    }

    public Models.Action Securitization(SecuritizationDto dto)
    {
        return actionFactory.Run(dto);
    }
}
