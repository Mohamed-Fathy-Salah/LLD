using InstallmentPlanner.DTOs;
using InstallmentPlanner.Strategies;

namespace InstallmentPlanner.Factories;

public class ActionFactory(
    InitiationAction initiation,
    CorridorAction corridor,
    EuriborAction euribor,
    SofrAction sofr,
    TerminationAction termination,
    OffloadingAction offloading,
    SecuritizationAction securitization
    )
{
    public Models.Action Run<T>(T dto) where T : BaseActionDto
    {
        return dto switch
        {
            InitiationDto => initiation.Run(dto),
            EuriborDto => euribor.Run(dto),
            SofrDto => sofr.Run(dto),
            CorridorDto => corridor.Run(dto),
            OffloadingDto => offloading.Run(dto),
            SecuritizationDto => securitization.Run(dto),
            TerminationDto => termination.Run(dto),
            _ => throw new ArgumentException($"Unsupported DTO type: {typeof(T).Name}")
        };
    }

    public Models.Action Rollback<T>(T dto) where T : BaseActionDto
    {
        return dto switch
        {
            InitiationDto => initiation.Rollback(dto),
            EuriborDto => euribor.Rollback(dto),
            SofrDto => sofr.Rollback(dto),
            CorridorDto => corridor.Rollback(dto),
            OffloadingDto => offloading.Rollback(dto),
            SecuritizationDto => securitization.Rollback(dto),
            TerminationDto => termination.Rollback(dto),
            _ => throw new ArgumentException($"Unsupported DTO type: {typeof(T).Name}")
        };
    }
}
