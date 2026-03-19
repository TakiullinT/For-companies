using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class DisconnectCommand : ICommand
{
    public string Name => "disconnect";

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        try
        {
            ICommandResult result = fileSystemManager.Disconnect();
            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось выполнить отключение: {exception.Message}");
        }
    }
}