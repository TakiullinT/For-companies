using Itmo.ObjectOrientedProgramming.Lab2.Loggers;

namespace Itmo.ObjectOrientedProgramming.Lab2.Notifications;

public class TextNotification : INotification
{
    private ILogger Logger { get; }

    public TextNotification(ILogger logger)
    {
        Logger = logger;
    }

    public void Notify(string message)
    {
        Console.WriteLine(message);
        Logger.Log($"Text notification sent: {message}");
    }
}
