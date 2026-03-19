using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.TopicInfo;

public class Topic : ITopic
{
    public string? Name { get; }

    private IList<IRecipient> Recipients { get; }

    public Topic(string name)
    {
        Name = name;
        Recipients = new List<IRecipient>();
    }

    public Result SendMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        foreach (IRecipient recipient in Recipients)
        {
            Result result = recipient.ReceiveMessage(message);
            if (!result.IsSuccess) return Result.Fail($"Recipent {recipient} failed");
        }

        return Result.Success();
    }
}
