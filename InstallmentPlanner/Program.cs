using Microsoft.Extensions.DependencyInjection;
using InstallmentPlanner.Configuration;
using InstallmentPlanner.Controllers;
using InstallmentPlanner.DTOs;
using InstallmentPlanner.Factories;

// Set up dependency injection
var serviceCollection = new ServiceCollection();
serviceCollection.AddInstallmentPlannerServices();

// Build service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

// Create controller with dependencies
var actionFactory = serviceProvider.GetRequiredService<ActionFactory>();
var controller = new Controller(actionFactory);

// Create sample initiation DTO
var initiationDto = new InitiationDto
{
    ContractId = 1,
    EffectiveChangeDate = DateTime.Now
};

try
{
    // Call the Initiate method
    Console.WriteLine("Calling controller.Initiate...");
    var result = controller.Initiate(initiationDto);

    Console.WriteLine($"Initiation completed successfully!");
    Console.WriteLine($"Action Type: {result?.Type}");
    Console.WriteLine($"Created At: {result?.CreatedAt}");
    Console.WriteLine($"Companies Count: {result?.Companies?.Length ?? 0}");

    Console.WriteLine(string.Join('\n', result?.Installments ?? []));
}
catch (Exception ex)
{
    Console.WriteLine($"Error during initiation: {ex.Message}");
}
