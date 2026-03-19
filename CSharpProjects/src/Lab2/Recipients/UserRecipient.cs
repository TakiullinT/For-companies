using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;
using Itmo.ObjectOrientedProgramming.Lab2.Users;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class UserRecipient : IRecipient
{
    private IUser User { get; }

    public UserRecipient(IUser user)
    {
        User = user;
    }

    public Result ReceiveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        return User.ReceiveMessage(message);
    }
}
