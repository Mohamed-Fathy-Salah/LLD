var logger = Logger.Instance;
logger.Config
    .WithMinLogLevel(LogLevel.INFO)
    .WithMaxLogLevel(LogLevel.ERROR)
    .WithDestinations([new ToConsole(), new ToFile("log.txt"), new ToFile("log2.txt")]);

logger.Debug("Debugging the system.");
logger.Info("System started.");
logger.Warning("Low disk space.");
logger.Error("Unable to load configuration.");
logger.Fatal("System crash imminent!");

logger.Config
    .WithFormatter(new LogLevelFirstFormatter())
    .WithMinLogLevel(LogLevel.FATAL);

logger.Debug("2 Debugging the system.");
logger.Info("2 System started.");
logger.Warning("2 Low disk space.");
logger.Error("2 Unable to load configuration.");
logger.Fatal("2 System crash imminent!");
