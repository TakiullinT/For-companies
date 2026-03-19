using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class FileCopyCommand : ICommand
{
    public string Name => "file copy";

    public string SourcePath { get; private set; }

    public string DestinationPath { get; private set; }

    public FileCopyCommand(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно произвести копирование: система не подключена");
        }

        try
        {
            ICommandResult result = fileSystemManager.CopyFile(SourcePath, DestinationPath);
            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось скопировать файл: {exception.Message}");
        }
    }
}