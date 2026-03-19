using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class RootParser
{
    private IReadOnlyList<ICommandParser> Parsers { get; }

    public RootParser(IReadOnlyList<ICommandParser> parsers)
    {
        Parsers = parsers;
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        foreach (ICommandParser parser in Parsers)
        {
            if (parser.CanParse(arguments))
            {
                return parser.Parse(arguments);
            }
        }

        return Result.Fail("Неизвестная команда");
    }
}