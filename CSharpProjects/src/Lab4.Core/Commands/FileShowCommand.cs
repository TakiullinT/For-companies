using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class FileShowCommand : ICommand
{
    public string Name => "file show";

    public string Path { get; private set; }

    public FileShowCommand(string path)
    {
        Path = path;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно вывести файл: система не подключена");
        }

        try
        {
            ICommandResult result = fileSystemManager.FileShow(Path);
            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось вывести файл {exception.Message}");
        }
    }
}