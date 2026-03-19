using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Factories;

public interface IFileSystemFactory
{
    ICommandResult Create(string mode);
}