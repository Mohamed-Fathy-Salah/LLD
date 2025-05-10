namespace VendingMachine;

public class Step(string name, Dictionary<int, int> ingredientsQuantity)
{
    public string Name { get; } = name;
    public Dictionary<int, int> IngredientsQuantity { get; } = ingredientsQuantity;

    public virtual void Execute()
    {
        Console.WriteLine("Executing step with ingredients" + string.Join("\n", IngredientsQuantity.Select(f => $"{f.Key} - {f.Value}")));
    }
}

public class AddMilkStep(string name, Dictionary<int, int> ingredientsQuantity) : Step(name, ingredientsQuantity)
{
    public override void Execute()
    {
        Console.WriteLine("adding milk");
    }
}
