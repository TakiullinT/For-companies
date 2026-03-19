using Itmo.ObjectOrientedProgramming.Lab2.Archiving;
using Itmo.ObjectOrientedProgramming.Lab2.Formatting;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;
using Itmo.ObjectOrientedProgramming.Lab2.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class ArchivingTests
{
    [Fact]
    public void SaveMessage_FormattingArchiver_ReturnsSuccess()
    {
        string expectedFormattedMessage = "# Test Header\n\nTest Body\n\n**Importance:** High";
        var mockFormatter = new MockFormatter(expectedFormattedMessage);

        string? actulOutput = null;

        Func<IMessageFormatter> formatterFactory = () => mockFormatter;
        Action<string> outputAction = message => actulOutput = message;

        var archiver = new FormattingArchiver(formatterFactory, outputAction);

        var message = new Message("Test Header", "Test Message", Message.Importance.High);

        Result result = archiver.SaveMessage(message);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void SaveMessage_FormattingArchiver_OutputIsCorrect()
    {
        string expectedFormattedMessage = "# Test Header\n\nTest Body\n\n**Importance:** High";
        var mockFormatter = new MockFormatter(expectedFormattedMessage);

        string? actulOutput = null;

        Func<IMessageFormatter> formatterFactory = () => mockFormatter;
        Action<string> outputAction = message => actulOutput = message;

        var archiver = new FormattingArchiver(formatterFactory, outputAction);

        var message = new Message("Test Header", "Test Message", Message.Importance.High);

        Result result = archiver.SaveMessage(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedFormattedMessage, actulOutput);
    }

    [Fact]
    public void SaveMessage_FormattingArchiver_WriteHeaderCounterIsCalled()
    {
        string expectedFormattedMessage = "# Test Header\n\nTest Body\n\n**Importance:** High";
        var mockFormatter = new MockFormatter(expectedFormattedMessage);

        string? actulOutput = null;

        Func<IMessageFormatter> formatterFactory = () => mockFormatter;
        Action<string> outputAction = message => actulOutput = message;

        var archiver = new FormattingArchiver(formatterFactory, outputAction);

        var message = new Message("Test Header", "Test Message", Message.Importance.High);

        Result result = archiver.SaveMessage(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedFormattedMessage, actulOutput);
        Assert.Equal(1, mockFormatter.WriteHeaderCounter);
    }

    [Fact]
    public void SaveMessage_FormattingArchiver_WriteBodyCounterIsCalled()
    {
        string expectedFormattedMessage = "# Test Header\n\nTest Body\n\n**Importance:** High";
        var mockFormatter = new MockFormatter(expectedFormattedMessage);

        string? actulOutput = null;

        Func<IMessageFormatter> formatterFactory = () => mockFormatter;
        Action<string> outputAction = message => actulOutput = message;

        var archiver = new FormattingArchiver(formatterFactory, outputAction);

        var message = new Message("Test Header", "Test Message", Message.Importance.High);

        Result result = archiver.SaveMessage(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedFormattedMessage, actulOutput);
        Assert.Equal(1, mockFormatter.WriteHeaderCounter);
        Assert.Equal(1, mockFormatter.WriteBodyCounter);
    }

    [Fact]
    public void SaveMessage_FormattingArchiver_WriteImportanceLevelCounterIsCalled()
    {
        string expectedFormattedMessage = "# Test Header\n\nTest Body\n\n**Importance:** High";
        var mockFormatter = new MockFormatter(expectedFormattedMessage);

        string? actulOutput = null;

        Func<IMessageFormatter> formatterFactory = () => mockFormatter;
        Action<string> outputAction = message => actulOutput = message;

        var archiver = new FormattingArchiver(formatterFactory, outputAction);

        var message = new Message("Test Header", "Test Message", Message.Importance.High);

        Result result = archiver.SaveMessage(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedFormattedMessage, actulOutput);
        Assert.Equal(1, mockFormatter.WriteHeaderCounter);
        Assert.Equal(1, mockFormatter.WriteBodyCounter);
        Assert.Equal(1, mockFormatter.WriteImportanceLevelCounter);
    }

    [Fact]
    public void SaveMessage_FormattingArchiver_GetFormattedMessageCounterIsCalled()
    {
        string expectedFormattedMessage = "# Test Header\n\nTest Body\n\n**Importance:** High";
        var mockFormatter = new MockFormatter(expectedFormattedMessage);

        string? actulOutput = null;

        Func<IMessageFormatter> formatterFactory = () => mockFormatter;
        Action<string> outputAction = message => actulOutput = message;

        var archiver = new FormattingArchiver(formatterFactory, outputAction);

        var message = new Message("Test Header", "Test Message", Message.Importance.High);

        Result result = archiver.SaveMessage(message);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedFormattedMessage, actulOutput);
        Assert.Equal(1, mockFormatter.WriteHeaderCounter);
        Assert.Equal(1, mockFormatter.WriteBodyCounter);
        Assert.Equal(1, mockFormatter.WriteImportanceLevelCounter);
        Assert.Equal(1, mockFormatter.GetFormattedMessageCounter);
    }
}