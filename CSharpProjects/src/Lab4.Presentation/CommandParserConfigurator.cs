using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class CommandParserConfigurator
{
    public static IReadOnlyList<ICommandParser> Build()
    {
        return new List<ICommandParser>
        {
            new ConnectParser(),
            new CopyParser(),
            new MoveParser(),
            new RenameParser(),
            new DeleteParser(),
            new DisconnectParser(),
            new ShowParser(),
            new TreeGoToParser(),
            new TreeListParser(),
        };
    }
}