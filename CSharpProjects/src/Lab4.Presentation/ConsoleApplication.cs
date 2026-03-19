using Itmo.ObjectOrientedProgramming.Lab4.Core.Factories;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.PathServices;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class ConsoleApplication
{
    public CommandLoop Loop { get; private set; }

    public ConsoleApplication()
    {
        IPath pathResolver = new PathService();
        IFileSystemFactory factory = new FileSystemFactory();

        var fileSystemManager = new FileSystemManager(factory, pathResolver);

        IReadOnlyList<ICommandParser> parsers = CommandParserConfigurator.Build();
        var rootParser = new RootParser(parsers);

        var renderer = new ConsoleRenderer();

        Loop = new CommandLoop(fileSystemManager, rootParser, renderer);
    }

    public void Run()
    {
        Loop.Run();
    }
}