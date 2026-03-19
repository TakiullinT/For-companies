using Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class TreeListCommand : ICommand
{
    public string Name => "tree list";

    public int Depth { get; private set; }

    public TreeListCommand(int depth)
    {
        Depth = depth;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (!fileSystemManager.State.IsConnected)
        {
            return Result.Fail("Невозможно вывести дерево: система не подключена");
        }

        try
        {
            if (fileSystemManager.State.LocalPath != null)
            {
                DirectoryEntry rootEntry = fileSystemManager.TreeList(fileSystemManager.State.LocalPath, Depth);
                return ResultType<DirectoryEntry>.Success(rootEntry, $"Содержимое каталога '{fileSystemManager.State.LocalPath}' до глубины {Depth}");
            }

            return ResultType<DirectoryEntry>.Fail($"Не удалось получить локальный путь из состояния");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось вывести дерево: {exception.Message}");
        }
    }
}