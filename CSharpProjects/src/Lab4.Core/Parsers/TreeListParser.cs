using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class TreeListParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 2 && arguments[0].Equals("tree", StringComparison.OrdinalIgnoreCase)
                                    && arguments[1].Equals("list", StringComparison.OrdinalIgnoreCase);
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к tree list");
        }

        int depth = 1;
        for (int i = 2; i < arguments.Count; i++)
        {
            if (arguments[i].Equals("-d", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= arguments.Count)
                {
                    return Result.Fail("Флаг -d указан без значения");
                }

                if (!int.TryParse(arguments[i + 1], out int parsedDepth) || parsedDepth < 1)
                {
                    return Result.Fail("Неверное значение глубины. Глубина должна быть положительным числом");
                }

                depth = parsedDepth;
                break;
            }
        }

        var command = new TreeListCommand(depth);
        return ResultType<ICommand>.Success(command, "Команда tree list успешно разобрана");
    }
}