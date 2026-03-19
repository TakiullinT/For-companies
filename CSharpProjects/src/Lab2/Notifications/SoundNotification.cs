using Itmo.ObjectOrientedProgramming.Lab2.Loggers;

namespace Itmo.ObjectOrientedProgramming.Lab2.Notifications;

public class SoundNotification : INotification
{
    private ILogger Logger { get; }

    private IBeepService Beeper { get; }

    public SoundNotification(ILogger logger, IBeepService beeper)
    {
        Logger = logger;
        Beeper = beeper;
    }

    public void Notify(string message)
    {
        try
        {
            Beeper.Beep();
            Logger.Log($"Sound notification sent: {message}");
        }
        catch (Exception exception)
        {
            Logger.Log($"Sound notification sent failed: {exception.Message}");
        }
    }
}
