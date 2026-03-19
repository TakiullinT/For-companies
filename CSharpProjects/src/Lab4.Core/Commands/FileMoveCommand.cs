using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class FileMoveCommand : ICommand
{
    public string Name => "file move";

    public string SourcePath { get; private set; }

    public string DestinationPath { get; private set; }

    public FileMoveCommand(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно переместить файл: система не подключена");
        }

        try
        {
            ICommandResult? result = fileSystemManager.FileMove(SourcePath, DestinationPath);
            if (result == null)
            {
                return Result.Fail("Ошибка: результат пустой");
            }

            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось переместить файл {exception.Message}");
        }
    }
}