namespace Linq;

public enum LogLevel { Verbose, Warning, Error };

public delegate void Logger(string input, LogLevel logLevel = LogLevel.Verbose);

public sealed class SubSystem
{
    private Logger _logger;

    public SubSystem(Logger logger)
    {
        _logger = logger;
    }

    public void Operation(string input)
    {
        _logger(input, LogLevel.Verbose);
    }
}
