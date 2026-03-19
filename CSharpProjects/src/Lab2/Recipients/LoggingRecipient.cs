using Itmo.ObjectOrientedProgramming.Lab2.Loggers;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class LoggingRecipient : IRecipient
{
    private IRecipient InnerRecipient { get; }

    private ILogger Logger { get; }

    public LoggingRecipient(IRecipient innerRecipient, ILogger logger)
    {
        InnerRecipient = innerRecipient;
        Logger = logger;
    }

    public Result ReceiveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        Logger.Log($"Message was received: {message}");
        return InnerRecipient.ReceiveMessage(message);
    }
}
