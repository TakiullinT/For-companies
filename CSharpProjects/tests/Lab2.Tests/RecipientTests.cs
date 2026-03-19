using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;
using Itmo.ObjectOrientedProgramming.Lab2.Tests.Mocks;
using Itmo.ObjectOrientedProgramming.Lab2.Users;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class RecipientTests
{
    [Fact]
    public void ReceiveMessage_LowImportanceFilteredRecipient_ReturnsFailure()
    {
        var mockRecipient = new MockRecipient();
        var filtered = new FilteredRecipient(mockRecipient, Message.Importance.High);

        var lowMessage = new Message("Test", "Low importance message", Message.Importance.Low);

        Result result = filtered.ReceiveMessage(lowMessage);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ReceiveMessage_LowImportanceFilteredRecipient_RecipientDidNotReceiveMessage()
    {
        var mockRecipient = new MockRecipient();
        var filtered = new FilteredRecipient(mockRecipient, Message.Importance.High);

        var lowMessage = new Message("Test", "Low importance message", Message.Importance.Low);

        Result result = filtered.ReceiveMessage(lowMessage);
        Assert.False(result.IsSuccess);
        Assert.Equal(0, mockRecipient.ReceivedCount);
    }

    [Fact]
    public void ReceiveMessage_LoggingRecipient_ReturnsSuccess()
    {
        var mockRecipient = new MockRecipient();
        var mockLogger = new MockLogger();
        var loggingRecipient = new LoggingRecipient(mockRecipient, mockLogger);

        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result result = loggingRecipient.ReceiveMessage(message);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ReceiveMessage_LoggingRecipient_LoggerIsCalled()
    {
        var mockRecipient = new MockRecipient();
        var mockLogger = new MockLogger();
        var loggingRecipient = new LoggingRecipient(mockRecipient, mockLogger);

        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result result = loggingRecipient.ReceiveMessage(message);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, mockLogger.LogCount);
    }

    [Fact]
    public void ReceiveMessage_LoggingRecipient_LoggerMessageIsCorrect()
    {
        var mockRecipient = new MockRecipient();
        var mockLogger = new MockLogger();
        var loggingRecipient = new LoggingRecipient(mockRecipient, mockLogger);

        var message = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result result = loggingRecipient.ReceiveMessage(message);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, mockLogger.LogCount);
        Assert.Equal($"Message was received: {message}", mockLogger.LastMessage);
    }

    [Fact]
    public void ReceiveMessage_FirstRecipient_MediumImportance_ReturnsSuccess()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);
    }

    [Fact]
    public void ReceiveMessage_FilteredSecondRecipient_BelowThreshold_ReturnsFailure()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);
        Result secondResult = filteredSecondRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);

        Assert.False(secondResult.IsSuccess);
    }

    [Fact]
    public void ReceiveMessage_FilteredSecondRecipient_BelowThreshold_ReturnsErrorMessage()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);
        Result secondResult = filteredSecondRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);

        Assert.False(secondResult.IsSuccess);
        Assert.Equal("Message importance is below allowed", secondResult.ErrorMessage);
    }

    [Fact]
    public void UserStoredMessages_AfterReceiving_ReturnsSingleMessage()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);
        Result secondResult = filteredSecondRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);

        Assert.False(secondResult.IsSuccess);
        Assert.Equal("Message importance is below allowed", secondResult.ErrorMessage);

        Assert.Single(user.StoredMessages);
    }

    [Fact]
    public void UserStoredMessages_AfterReceiving_ReturnsCorrectHeader()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);
        Result secondResult = filteredSecondRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);

        Assert.False(secondResult.IsSuccess);
        Assert.Equal("Message importance is below allowed", secondResult.ErrorMessage);

        Assert.Single(user.StoredMessages);
        Assert.Equal("Test Header", user.StoredMessages[0].Message.Header);
    }

    [Fact]
    public void UserStoredMessages_GetMessageStatus_ReturnsSuccess()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);
        Result secondResult = filteredSecondRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);

        Assert.False(secondResult.IsSuccess);
        Assert.Equal("Message importance is below allowed", secondResult.ErrorMessage);

        Assert.Single(user.StoredMessages);
        Assert.Equal("Test Header", user.StoredMessages[0].Message.Header);

        ResultType<MessageStatus> status = user.GetMessageStatus(mediumLevelMessage);
        Assert.True(status.IsSuccess);
    }

    [Fact]
    public void GetMessageStatus_MediumMessage_ReturnsUnread()
    {
        var user = new User("TestUser");

        var firstRecipient = new UserRecipient(user);
        var secondRecipient = new UserRecipient(user);

        var filteredSecondRecipient = new FilteredRecipient(secondRecipient, Message.Importance.High);

        var mediumLevelMessage = new Message("Test Header", "Test Message", Message.Importance.Medium);

        Result firstResult = firstRecipient.ReceiveMessage(mediumLevelMessage);
        Result secondResult = filteredSecondRecipient.ReceiveMessage(mediumLevelMessage);

        Assert.True(firstResult.IsSuccess);

        Assert.False(secondResult.IsSuccess);
        Assert.Equal("Message importance is below allowed", secondResult.ErrorMessage);

        Assert.Single(user.StoredMessages);
        Assert.Equal("Test Header", user.StoredMessages[0].Message.Header);

        ResultType<MessageStatus> status = user.GetMessageStatus(mediumLevelMessage);
        Assert.True(status.IsSuccess);
        Assert.Equal(MessageStatus.Unread, status.Value);
    }
}
