using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class RenameParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 3 && (arguments[0].Equals("rename", StringComparison.OrdinalIgnoreCase) ||
                                        (arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
                                         arguments[1].Equals("rename", StringComparison.OrdinalIgnoreCase)));
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к rename");
        }

        int startIndex = arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        string sourcePath = arguments[startIndex + 1].Trim().Trim('"');
        string newName = arguments[startIndex + 2].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return Result.Fail("Не указан исходный путь к файлу для переименования");
        }

        if (string.IsNullOrWhiteSpace(newName))
        {
            return Result.Fail("Не указано новое имя для файла");
        }

        var command = new FileRenameCommand(sourcePath, newName);
        return ResultType<ICommand>.Success(command, "Команда file rename разобрана");
    }
}