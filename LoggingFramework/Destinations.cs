public interface IDestination
{
    public void Write(string message);
}

public class ToConsole : IDestination
{
    public void Write(string message) =>
        Console.WriteLine(message);
}

public class ToFile(string path) : IDestination
{
    public void Write(string message) =>
        File.AppendAllText(path, message + Environment.NewLine);
}
