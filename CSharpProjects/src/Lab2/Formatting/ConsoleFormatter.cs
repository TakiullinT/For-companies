using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;
using System.Text;

namespace Itmo.ObjectOrientedProgramming.Lab2.Formatting;

public class ConsoleFormatter : IMessageFormatter
{
    private StringBuilder FormattedMessage { get; }

    public ConsoleFormatter()
    {
        FormattedMessage = new StringBuilder();
    }

    public Result WriteHeader(string header)
    {
        if (string.IsNullOrEmpty(header)) return Result.Fail("Empty header");
        FormattedMessage.AppendLine($"# {header}");
        return Result.Success();
    }

    public Result WriteBody(string body)
    {
        if (string.IsNullOrEmpty(body)) return Result.Fail("Empty body");
        FormattedMessage.AppendLine();
        FormattedMessage.AppendLine(body);
        return Result.Success();
    }

    public Result WriteImportance(Message.Importance importance)
    {
        if (importance < Message.Importance.Low) return Result.Fail("Low importance");
        FormattedMessage.AppendLine();
        FormattedMessage.AppendLine($"**Importance:** {importance}");
        return Result.Success();
    }

    public string GetFormattedMessage() => FormattedMessage.ToString();

    public void Reset() => FormattedMessage.Clear();
}
