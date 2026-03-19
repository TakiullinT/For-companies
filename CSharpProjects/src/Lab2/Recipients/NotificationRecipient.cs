using Itmo.ObjectOrientedProgramming.Lab2.Notifications;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class NotificationRecipient : IRecipient
{
    private INotification Notification { get; }

    private IEnumerable<string> Keywords { get; }

    public NotificationRecipient(INotification notification, IEnumerable<string> keywords)
    {
        Notification = notification;
        Keywords = keywords;
    }

    public Result ReceiveMessage(Message message)
    {
        if (message == null || message.Header == null || message.Body == null) return Result.Fail("Message is null");
        string text = message.Header + " " + message.Body;
        foreach (string keyword in Keywords)
        {
            if (text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                Notification.Notify($"Keyword '{keyword}' found in message");
                return Result.Fail("Keyword found in message");
            }
        }

        return Result.Success();
    }
}
