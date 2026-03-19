using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class FileRenameCommand : ICommand
{
    public string Name => "file rename";

    public string Path { get; private set; }

    public string NewName { get; private set; }

    public FileRenameCommand(string path, string newName)
    {
        Path = path;
        NewName = newName;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно переименовать файл: система не подключена");
        }

        try
        {
            ICommandResult result = fileSystemManager.RenameFile(Path, NewName);
            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось переименовать файл {exception.Message}");
        }
    }
}