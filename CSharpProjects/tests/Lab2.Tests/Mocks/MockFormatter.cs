using Itmo.ObjectOrientedProgramming.Lab2.Formatting;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests.Mocks;

public class MockFormatter : IMessageFormatter
{
    private readonly string _formattedMessage;

    public int WriteHeaderCounter { get; private set; }

    public int WriteBodyCounter { get; private set; }

    public int WriteImportanceLevelCounter { get; private set; }

    public int GetFormattedMessageCounter { get; private set; }

    public MockFormatter(string formattedMessage)
    {
        _formattedMessage = formattedMessage;
    }

    public Result WriteHeader(string header)
    {
        WriteHeaderCounter++;
        return Result.Success();
    }

    public Result WriteBody(string body)
    {
        WriteBodyCounter++;
        return Result.Success();
    }

    public Result WriteImportance(Message.Importance importance)
    {
        WriteImportanceLevelCounter++;
        return Result.Success();
    }

    public string GetFormattedMessage()
    {
        GetFormattedMessageCounter++;
        return _formattedMessage;
    }
}