using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class GroupRecipient : IRecipient
{
    public IEnumerable<IRecipient> StoredRecipients => Recipients;

    private List<IRecipient> Recipients { get; }

    public GroupRecipient()
    {
        Recipients = new List<IRecipient>();
    }

    public void AddRecipient(IRecipient recipient)
    {
        Recipients.Add(recipient);
    }

    public void RemoveRecipient(IRecipient recipient)
    {
        Recipients.Remove(recipient);
    }

    public Result ReceiveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        foreach (IRecipient recipient in Recipients)
        {
            Result result = recipient.ReceiveMessage(message);
            if (!result.IsSuccess) return Result.Fail("One of recipients failed to receive message");
        }

        return Result.Success();
    }
}
