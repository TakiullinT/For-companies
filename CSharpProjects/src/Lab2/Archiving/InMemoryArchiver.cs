using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Archiving;

public class InMemoryArchiver : IMessageArchiver
{
    public IReadOnlyList<Message> StoredMessages => Messages.AsReadOnly();

    private List<Message> Messages { get; }

    public InMemoryArchiver(IEnumerable<Message> messages)
    {
        Messages = messages != null ? new List<Message>(messages) : new List<Message>();
    }

    public Result SaveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        try
        {
            Messages.Add(message);
            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Fail($"Save message failed {exception.Message}");
        }
    }
}
