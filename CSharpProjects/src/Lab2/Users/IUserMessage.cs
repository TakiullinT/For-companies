using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Users;

public enum MessageStatus
{
    Read,
    Unread,
}

public interface IUserMessage
{
    Message Message { get; }

    MessageStatus Status { get; }

    Result MarkAsRead();
}