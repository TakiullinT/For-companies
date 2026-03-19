using Itmo.ObjectOrientedProgramming.Lab2.Loggers;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests.Mocks;

public class MockLogger : ILogger
{
    public int LogCount { get; private set; }

    public string? LastMessage { get; private set; }

    public void Log(string message)
    {
        LogCount++;
        LastMessage = message;
    }
}