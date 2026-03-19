using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;
using Itmo.ObjectOrientedProgramming.Lab2.Users;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class UserTests
{
    [Fact]
    public void ReceiveMessage_UserMessage_ReturnsSuccess()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result result = user.ReceiveMessage(message);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GetMessageStatus_NewMessage_ReturnsSuccess()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result result = user.ReceiveMessage(message);
        Assert.True(result.IsSuccess);

        ResultType<MessageStatus> status = user.GetMessageStatus(message);
        Assert.True(status.IsSuccess);
    }

    [Fact]
    public void GetMessageStatus_NewMessage_ReturnsUnread()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result result = user.ReceiveMessage(message);
        Assert.True(result.IsSuccess);

        ResultType<MessageStatus> status = user.GetMessageStatus(message);
        Assert.True(status.IsSuccess);
        Assert.Equal(MessageStatus.Unread, status.Value);
    }

    [Fact]
    public void MarkMessageAsRead_ReceivedMessage_ReturnsSuccess()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        user.ReceiveMessage(message);

        Result result = user.MarkMessageAsRead(message);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GetMessageStatus_ReadMessage_ReturnsSuccess()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        user.ReceiveMessage(message);

        Result result = user.MarkMessageAsRead(message);
        Assert.True(result.IsSuccess);

        ResultType<MessageStatus> status = user.GetMessageStatus(message);
        Assert.True(status.IsSuccess);
    }

    [Fact]
    public void GetMessageStatus_ReadMessage_ReturnsRead()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        user.ReceiveMessage(message);

        Result result = user.MarkMessageAsRead(message);
        Assert.True(result.IsSuccess);

        ResultType<MessageStatus> status = user.GetMessageStatus(message);
        Assert.True(status.IsSuccess);
        Assert.Equal(MessageStatus.Read, status.Value);
    }

    [Fact]
    public void MarkMessageAsRead_AlreadyReadMessage_ReturnsFailure()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        user.ReceiveMessage(message);
        user.MarkMessageAsRead(message);

        Result result = user.MarkMessageAsRead(message);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void MarkMessageAsRead_AlreadyReadMessage_ReturnsErrorMessage()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        user.ReceiveMessage(message);
        user.MarkMessageAsRead(message);

        Result result = user.MarkMessageAsRead(message);
        Assert.False(result.IsSuccess);
        Assert.Equal("Message already read", result.ErrorMessage);
    }

    [Fact]
    public void GetMessageStatus_AlreadyReadMessage_ReturnsRead()
    {
        var user = new User("TestUser");
        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        user.ReceiveMessage(message);
        user.MarkMessageAsRead(message);

        Result result = user.MarkMessageAsRead(message);
        Assert.False(result.IsSuccess);
        Assert.Equal("Message already read", result.ErrorMessage);

        ResultType<MessageStatus> status = user.GetMessageStatus(message);
        Assert.True(status.IsSuccess);
    }
}