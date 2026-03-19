using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public class ConnectCommand : ICommand
{
    public string Name => "connect";

    public string Address { get; private set; }

    public string Mode { get; private set; }

    public ConnectCommand(string address, string mode)
    {
        if (string.IsNullOrEmpty(address))
        {
            Result.Fail("Адрес не может быть пустым");
        }

        Address = address;
        Mode = mode;
    }

    public ICommandResult Execute(FileSystemManager fileSystemManager)
    {
        if (fileSystemManager.State.IsConnected)
        {
            Result.Fail("Система уже подключена");
        }

        try
        {
            ICommandResult? result = fileSystemManager.Connect(Address, Mode);
            if (result == null)
            {
                return Result.Fail("Connect вернул пустой результат");
            }

            return result;
        }
        catch (Exception exception)
        {
            return Result.Fail($"Не удалось выполнить подключение: {exception.Message}");
        }
    }
}