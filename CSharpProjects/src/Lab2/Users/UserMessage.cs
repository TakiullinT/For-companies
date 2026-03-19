using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Users;

public class UserMessage : IUserMessage
{
    public Message Message { get; }

    public MessageStatus Status { get; private set; }

    public UserMessage(Message message)
    {
        Message = message;
        Status = MessageStatus.Unread;
    }

    public Result MarkAsRead()
    {
        if (Status == MessageStatus.Read) return Result.Fail("Message already read");
        Status = MessageStatus.Read;
        return Result.Success();
    }
}