public class LogConfig
{
    public LogLevel MinLevel { get; private set; } = LogLevel.DEBUG;
    public LogLevel MaxLevel { get; private set; } = LogLevel.FATAL;
    public IDestination[] Destinations { get; private set; } = [];
    public IFormatter Formatter { get; private set; } = new TimeStampFirstFormatter();

    public LogConfig WithMinLogLevel(LogLevel level)
    {
        MinLevel = level;
        if (MinLevel > MaxLevel) MaxLevel = level;
        return this;
    }
    public LogConfig WithMaxLogLevel(LogLevel level)
    {
        MaxLevel = level;
        if (MaxLevel < MinLevel) MinLevel = level;
        return this;
    }
    public LogConfig WithDestinations(IDestination[] destinations)
    {
        Destinations = destinations;
        return this;
    }
    public LogConfig WithFormatter(IFormatter formatter)
    {
        Formatter = formatter;
        return this;
    }
}
