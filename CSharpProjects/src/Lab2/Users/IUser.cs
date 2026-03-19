using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Users;

public interface IUser
{
    Result ReceiveMessage(Message message);

    ResultType<MessageStatus> GetMessageStatus(Message message);

    Result MarkMessageAsRead(Message message);
}