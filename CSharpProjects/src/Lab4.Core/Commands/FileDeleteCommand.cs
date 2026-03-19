using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class FileDeleteCommand : ICommand
{
    public string Name => "file delete";

    public string Path { get; private set; }

    public FileDeleteCommand(string path)
    {
        Path = path;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно удалить файл: система не подключена");
        }

        try
        {
            ICommandResult result = fileSystemManager.DeleteFile(Path);
            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось удалить файл {exception.Message}");
        }
    }
}