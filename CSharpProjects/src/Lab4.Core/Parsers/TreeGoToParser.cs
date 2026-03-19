using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class TreeGoToParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 3 &&
               arguments[0].Equals("tree", StringComparison.OrdinalIgnoreCase) &&
               arguments[1].Equals("goto", StringComparison.OrdinalIgnoreCase);
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        string tagetPath = arguments[2].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(tagetPath))
        {
            return Result.Fail("Не указан целевой путь для перехода");
        }

        ICommand command = new TreeGoToCommand(tagetPath);
        return ResultType<ICommand>.Success(command, "Команда tree goto разобрана");
    }
}