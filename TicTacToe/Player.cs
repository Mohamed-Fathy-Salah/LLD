public abstract class Player(string name, char symbol)
{
    public string Name { get; } = name;
    public char Symbol { get; } = symbol;
    public int Play()
    {
        Console.WriteLine(this + " Turn!");
        return GetPlay();
    }
    protected abstract int GetPlay();
    public override string ToString() => $"Player {Symbol} : {Name}";
}

public class HumanPlayer(string name, char symbol) : Player(name, symbol)
{
    protected override int GetPlay()
    {
        Console.Write("Enter a number from the board:");
        return int.TryParse(Console.ReadLine(), out int number) ? number : -1;
    }
}

public class AIPlayer(string name, char symbol) : Player(name, symbol)
{
    protected override int GetPlay()
    {
        throw new NotImplementedException();
    }
}
