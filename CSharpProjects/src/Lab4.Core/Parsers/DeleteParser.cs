using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class DeleteParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 2 &&
               (arguments[0].Equals("delete", StringComparison.OrdinalIgnoreCase) ||
                (arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
                 arguments[1].Equals("delete", StringComparison.OrdinalIgnoreCase)));
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к delete");
        }

        int startIndex = arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

        if (arguments.Count <= startIndex + 1)
        {
            return Result.Fail("Не указан путь к файлу для удаления");
        }

        string sourcePath = arguments[startIndex + 1].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return Result.Fail("Не указан путь к файлу для удаления");
        }

        var command = new FileDeleteCommand(sourcePath);
        return ResultType<ICommand>.Success(command, "Команда file delete разобрана");
    }
}