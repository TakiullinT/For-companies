using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class FilteredRecipient : IRecipient
{
    private IRecipient InnerRecipient { get; }

    private Message.Importance MinImportanceLevel { get; }

    public FilteredRecipient(IRecipient innerRecipient, Message.Importance minImportanceLevel)
    {
        InnerRecipient = innerRecipient;
        MinImportanceLevel = minImportanceLevel;
    }

    public Result ReceiveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        if (message.Level < MinImportanceLevel) return Result.Fail("Message importance is below allowed");
        return InnerRecipient.ReceiveMessage(message);
    }
}
