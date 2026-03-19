using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public interface ICommand
{
    string Name { get; }

    ICommandResult Execute(FileSystemManager fileSystemManager);
}