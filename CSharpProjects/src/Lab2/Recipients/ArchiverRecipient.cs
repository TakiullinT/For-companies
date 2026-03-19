using Itmo.ObjectOrientedProgramming.Lab2.Archiving;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class ArchiverRecipient : IRecipient
{
    private IMessageArchiver? MessageAchiver { get; }

    public ArchiverRecipient(IMessageArchiver? messageAchiver)
    {
        MessageAchiver = messageAchiver;
    }

    public Result ReceiveMessage(Message message)
    {
        if (MessageAchiver is null) return Result.Fail("No message achiver");
        if (message is null) return Result.Fail("Message is null");
        try
        {
            MessageAchiver.SaveMessage(message);
            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Fail($"Failed to save message: {exception.Message}");
        }
    }
}
