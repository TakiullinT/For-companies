using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class ConnectParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 2 && arguments[0].Equals("connect", StringComparison.OrdinalIgnoreCase);
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к connect");
        }

        string address = arguments[1].Trim().Trim('"');
        if (address.StartsWith('-'))
        {
            return Result.Fail("Не указан адрес подключения или указан некорректно (ожидался путь, а получен флаг).");
        }

        string mode = "local";

        for (int i = 2; i < arguments.Count; i++)
        {
            if (arguments[i].Equals("-m", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= arguments.Count)
                {
                    return Result.Fail("Флаг -m указан, но режим не задан.");
                }

                mode = arguments[i + 1].Trim().Trim('"');
                break;
            }
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            return Result.Fail("Не указан адрес подключения");
        }

        if (string.IsNullOrWhiteSpace(mode))
        {
            return Result.Fail("Не указан режим работы");
        }

        var command = new ConnectCommand(address, mode);

        return ResultType<ICommand>.Success(command, "Команда connect разобрана");
    }
}