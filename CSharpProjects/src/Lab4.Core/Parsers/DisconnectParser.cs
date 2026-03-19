using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class DisconnectParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count == 1 && arguments[0].Equals("disconnect", StringComparison.OrdinalIgnoreCase);
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        var command = new DisconnectCommand();
        return ResultType<ICommand>.Success(command, "Команда disconnect разобрана");
    }
}