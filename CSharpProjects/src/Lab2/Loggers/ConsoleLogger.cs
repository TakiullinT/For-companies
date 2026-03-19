namespace Itmo.ObjectOrientedProgramming.Lab2.Loggers;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}