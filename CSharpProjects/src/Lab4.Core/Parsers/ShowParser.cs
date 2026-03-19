using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class ShowParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 2 && (arguments[0].Equals("show", StringComparison.OrdinalIgnoreCase) ||
                                        (arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
                                         arguments[1].Equals("show", StringComparison.OrdinalIgnoreCase)));
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к show");
        }

        int startIndex = arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        string path = arguments[startIndex + 1].Trim().Trim('"');
        string mode = string.Empty;

        for (int i = 2; i < arguments.Count; i++)
        {
            if (arguments[i].Equals("-m", StringComparison.OrdinalIgnoreCase) && i + 1 < arguments.Count)
            {
                mode = arguments[i + 1].Trim().Trim('"');
                break;
            }
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            return Result.Fail("Не указан путь к файлу для отображения");
        }

        if (string.IsNullOrWhiteSpace(mode))
        {
            return Result.Fail("Обязательный флаг '-m' не найден или не указан режим");
        }

        var command = new FileShowCommand(path);
        return ResultType<ICommand>.Success(command, "Команда file show разобрана");
    }
}