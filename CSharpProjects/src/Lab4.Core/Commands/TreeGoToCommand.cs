using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class TreeGoToCommand : ICommand
{
    public string Name => "tree go to";

    public string Path { get; private set; }

    public TreeGoToCommand(string path)
    {
        Path = path;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно сменить каталог: система не подключена");
        }

        try
        {
            ICommandResult result = fileSystemManager.Goto(Path);
            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось выполнить переход: {exception.Message}");
        }
    }
}