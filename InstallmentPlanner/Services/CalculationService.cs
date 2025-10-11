using InstallmentPlanner.UnitOfWork;
using InstallmentPlanner.Models;

namespace InstallmentPlanner.Services;

public class CalculationService(IUnitOfWork unitOfWork) : ICalculationService
{
    public void GeneratePlanSkeleton(Models.Action action)
    {
        //inadvance
        DateOnly currentDate = action.InstallmentGroupSpecs.FirstPaymentDate;
        var interestRate = action.InstallmentGroupSpecs.Margin + action.InstallmentGroupSpecs.CbeRate;
        for (int i = 1; i <= action.InstallmentGroupSpecs.NumberOfInstallments; i++)
        {
            var newDate = currentDate.AddMonths(action.InstallmentGroupSpecs.GetNumberOfMonths());
            var numberOfDays = (newDate.ToDateTime(TimeOnly.MinValue) - currentDate.ToDateTime(TimeOnly.MinValue)).Days;
            action.Installments.Add(new(i, currentDate, numberOfDays, interestRate));
            currentDate = newDate;
        }
    }

    public void Break(Installment installment)
    {
        throw new NotImplementedException();
    }

    public void GenerateAccrual(Installment installment)
    {
        throw new NotImplementedException();
    }

    public void GenerateAccrual(InstallmentDetail installment)
    {
        throw new NotImplementedException();
    }

    public void PopulateRent(Models.Action action, decimal previousClosing, decimal rent)
    {
        //inadvance
        foreach (var i in action.Installments)
        {
            rent *= action.InstallmentGroupSpecs.PeriodsStepPercentage.GetValueOrDefault(i.Period, 1);
            if (action.InstallmentGroupSpecs.FullCapitalizationPeriods.Contains(i.Period)) i.Rent = 0;
            else i.Rent = rent;
            i.Opening = previousClosing;
            i.InterestAmount = (i.Opening - i.Rent) * i.NumberOfDays * i.InterestRate / 360;
            i.PrincipalAmount = i.Rent - i.InterestAmount;
            previousClosing = i.Closing = previousClosing - i.PrincipalAmount;
        }
    }

    public void GoalSeek(Models.Action action)
    {
        var NFA = action.Companies.Sum(f => f.ShareAmount);
        decimal minRent = 0, maxRent = NFA * 2, midRent;
        while (maxRent - minRent > 0.0001m)
        {
            midRent = (maxRent + minRent) / 2;
            PopulateRent(action, NFA, midRent);
            if (action.Installments.Last().Closing > 0) minRent = midRent;
            else maxRent = midRent;
        }
    }

    public void MakePlan(Models.Action action)
    {
        GeneratePlanSkeleton(action);
        GoalSeek(action);
        //action.Installments.ForEach(f =>
        //{
        //GenerateAccrual(f);
        //Break(f);
        //f.InstallmentDetails.ForEach(GenerateAccrual);
        //});
    }
}
