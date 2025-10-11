using InstallmentPlanner.UnitOfWork;
using InstallmentPlanner.Factories;

namespace InstallmentPlanner.Services;

public class CalculationService(IUnitOfWork unitOfWork, ActionFactory actionFactory) : ICalculationService
{
}
