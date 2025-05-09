public interface IFormatter
{
    public string Format(LogMessage message);
}

public class TimeStampFirstFormatter : IFormatter
{
    public string Format(LogMessage message) =>
        $"{message.timeStamp} - {message.level} - {message.message}";
}

public class LogLevelFirstFormatter : IFormatter
{
    public string Format(LogMessage message) =>
        $"{message.level} - {message.timeStamp} - {message.message}";
}
