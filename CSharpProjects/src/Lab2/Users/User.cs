using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Users;

public class User : IUser
{
    public string Name { get; }

    public IReadOnlyList<IUserMessage> StoredMessages => Messages.AsReadOnly();

    private List<IUserMessage> Messages { get; }

    public User(string name)
    {
        Name = name;
        Messages = new List<IUserMessage>();
    }

    public Result ReceiveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");

        if (Messages.Any(m =>
                m.Message.Header == message.Header))
        {
            return Result.Fail("Duplicate message");
        }

        Messages.Add(new UserMessage(message));
        return Result.Success();
    }

    public ResultType<MessageStatus> GetMessageStatus(Message message)
    {
        if (message is null) return ResultType<MessageStatus>.Fail("Message is null");
        IUserMessage? userMessage = Messages.FirstOrDefault(m => m.Message == message);
        if (userMessage is null) return ResultType<MessageStatus>.Fail("UserMessage is null");
        return ResultType<MessageStatus>.Success(userMessage.Status);
    }

    public Result MarkMessageAsRead(Message message)
    {
        IUserMessage? userMessage = Messages.FirstOrDefault(m => m.Message == message);
        if (userMessage is null) return Result.Fail("UserMessage is null");
        return userMessage.MarkAsRead();
    }
}
