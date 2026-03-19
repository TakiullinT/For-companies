using Itmo.ObjectOrientedProgramming.Lab2.Formatting;
using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Archiving;

public class FormattingArchiver : IMessageArchiver
{
    private Func<IMessageFormatter> FormatterFactory { get; }

    private Action<string> OutputAction { get; }

    public FormattingArchiver(Func<IMessageFormatter> formatterFactory, Action<string> outputAction)
    {
        FormatterFactory = formatterFactory;
        OutputAction = outputAction;
    }

    public Result SaveMessage(Message message)
    {
        if (message is null) return Result.Fail("Message is null");
        IMessageFormatter formatter;
        try
        {
            formatter = FormatterFactory();
            if (formatter is null) return Result.Fail("Formatter is null");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Formatter factory throw {exception.Message}");
        }

        Result result = formatter.WriteHeader(message.Header);
        if (!result.IsSuccess) return Result.Fail($"WriteHeader failed");
        result = formatter.WriteBody(message.Body);
        if (!result.IsSuccess) return Result.Fail($"WriteBody failed");

        try
        {
            result = formatter.WriteImportance(message.Level);
            if (!result.IsSuccess) return Result.Fail($"WriteImportance failed");
        }
        catch (Exception exception)
        {
            return Result.Fail($"WriteImportance failed {exception.Message}");
        }

        string formattedMessage = formatter.GetFormattedMessage();
        if (string.IsNullOrEmpty(formattedMessage)) return Result.Fail("FormattedMessage is null or empty");
        try
        {
            OutputAction(formattedMessage);
            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Fail($"Output action failed {exception.Message}");
        }
    }
}
