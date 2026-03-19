using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests.Mocks;

public class MockRecipient : IRecipient
{
    public int ReceivedCount { get; private set; } = 0;

    public Message? LastReceivedMessage { get; private set; }

    public Result ReceiveMessage(Message message)
    {
        ReceivedCount++;
        LastReceivedMessage = message;
        return Result.Success();
    }
}