using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public interface ICommandParser
{
    bool CanParse(IReadOnlyList<string> arguments);

    ICommandResult Parse(IReadOnlyList<string> arguments);
}