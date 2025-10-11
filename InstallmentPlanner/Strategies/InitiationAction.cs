using InstallmentPlanner.DTOs;
using InstallmentPlanner.Enums;
using InstallmentPlanner.Models;
using InstallmentPlanner.Services;

namespace InstallmentPlanner.Strategies;

public class InitiationAction(ICalculationService calculationService) : IActionStrategy
{
    public ActionTypeEnum Type => ActionTypeEnum.Initiation;

    public Models.Action Run(BaseActionDto dto)
    {
        var action = new Models.Action(
                new(margin: 0.06m,
                    cbeRate: 0.2m,
                    numberOfInstallments: 12,
                    firstPaymentDate: new DateOnly(2024, 1, 1),
                    servingInterestPeriods: [],
                    fullCapitalizationPeriods: [],
                    periodsStepPercentage: new Dictionary<int, decimal> { [5] = 2m, },
                    repaymentStructure: RepaymentStructureEnum.Monthly),
                [new Company(
                    companyId: 1,
                    shareAmount: 30000m,
                    sharePercentage: 1m,
                    isMainCollector: true,
                    shouldCreateJournals: true,
                    includedPeriods: new[] {1,2,3,4,5,6,7,8,9,10,11,12}
                    )], DateTime.UtcNow, ActionTypeEnum.Initiation);
        calculationService.MakePlan(action);
        return action;
    }

    public Models.Action Rollback(BaseActionDto dto)
    {
        // TODO: Implement initiation rollback logic
        throw new NotImplementedException();
    }
}
