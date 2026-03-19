using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class CommandLoop
{
    public FileSystemManager FileSystemManager { get; private set; }

    public RootParser RootCommandParser { get; private set; }

    public ConsoleRenderer Renderer { get; private set; }

    public CommandLoop(FileSystemManager fileSystemManager, RootParser rootCommandParser, ConsoleRenderer renderer)
    {
        FileSystemManager = fileSystemManager;
        RootCommandParser = rootCommandParser;
        Renderer = renderer;
    }

    public void Run()
    {
        Console.WriteLine("Введите 'exit' или 'disconnect' для выхода.");
        while (true)
        {
            string commandPrompt = FileSystemManager.State.IsConnected
                ? $"[{FileSystemManager.State.ActiveMode}] {FileSystemManager.State.LocalPath}>"
                : ">";
            Console.WriteLine(commandPrompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            ICommandResult parseResult = RootCommandParser.Parse(input.Split(' '));

            if (!parseResult.IsSuccess || parseResult is not ResultType<ICommand> commandResult)
            {
                Renderer.RenderResult(parseResult);
                continue;
            }

            ICommand? commandToExecute = commandResult.Value;
            if (commandToExecute == null)
            {
                continue;
            }

            ICommandResult executionResult = commandToExecute.Execute(FileSystemManager);

            Renderer.RenderResult(executionResult);
        }

        Console.WriteLine("Приложение завершено");
    }
}
