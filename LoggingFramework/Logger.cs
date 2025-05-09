public class Logger
{
    private static readonly Lazy<Logger> instance = new(() => new Logger());
    public static Logger Instance => instance.Value;
    private Logger()
    {
        Config = new LogConfig();
    }

    public LogConfig Config { get; set; }
    public void Log(LogLevel level, string message)
    {
        if (level >= Config.MinLevel && level <= Config.MaxLevel)
            foreach (var destination in Config.Destinations)
                destination.Write(Config.Formatter.Format(new LogMessage(DateTime.UtcNow, level, message)));
    }
    public void Fatal(string message) => Log(LogLevel.FATAL, message);
    public void Error(string message) => Log(LogLevel.ERROR, message);
    public void Warning(string message) => Log(LogLevel.WARNING, message);
    public void Info(string message) => Log(LogLevel.INFO, message);
    public void Debug(string message) => Log(LogLevel.DEBUG, message);
}
