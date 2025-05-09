# Designing a Logging Framework

## Requirements
1. The logging framework should support different log levels, such as DEBUG, INFO, WARNING, ERROR, and FATAL.
2. It should allow logging messages with a timestamp, log level, and message content.
3. The framework should support multiple output destinations, such as console, file, and database.
4. It should provide a configuration mechanism to set the log level and output destination.
5. The logging framework should be thread-safe to handle concurrent logging from multiple threads.
6. It should be extensible to accommodate new log levels and output destinations in the future.

```mermaid
classDiagram
    class Logger {
        LogConfig Config
        void Log(LogLevel level, string message)
        void Fatal(string message)
        void Error(string message)
        void Warning(string message)
        void Info(string message)
        void Debug(string message)
    }
    class LogLevel {
        <<enumeration>>
        FATAL
        ERROR
        WARNING
        INFO
        DEBUG
    }
    class LogConfig {
        LogLevel MinLevel
        LogLevel MaxLevel
        bool ShouldPrintTimeStamp
        bool ShouldPrintLogLevel
        IDestination[] destinations
        IFormatter formatter
        LogConfig WithMinLogLevel(LogLevel level)
        LogConfig WithMaxLogLevel(LogLevel level)
        LogConfig WithTimeStamp()
        LogConfig WithLogLevel()
        LogConfig WithDestinations(IDestination[] destinations)
        LogConfig WithFormatter(IFormatter formatter)
    }
    class IFormatter {
        <<interface>>
        string Format(LogMessage message)
    }
    class TimeStampFirstFormatter {
        string Format(LogMessage message)
    }
    class LogLevelFirstFormatter {
        string Format(LogMessage message)
    }
    class IDestination {
        <<interface>>
        void Write(string message)
    }
    class ToConsole {
        void Write(string message)
    }
    class ToFile {
        string path
        void Write(string message)
    }
    class ToDatabase {
        string connectionString
        void Write(string message)
    }
    class LogMessage {
        DateTime Timestamp
        LogLevel Level
        string Message
    }
    Logger --> LogConfig
    LogConfig --> IDestination
    LogConfig --> IFormatter
    IFormatter <|.. TimeStampFirstFormatter
    IFormatter <|.. LogLevelFirstFormatter
    IDestination <|.. ToConsole
    IDestination <|.. ToFile
    IDestination <|.. ToDatabase
```
