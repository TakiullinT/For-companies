using Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Factories;

public class FileSystemFactory : IFileSystemFactory
{
    public ICommandResult Create(string mode)
    {
        if (mode.Equals("local", StringComparison.OrdinalIgnoreCase))
        {
            var provider = new LocalFileSystemProvider();
            return ResultType<IFileSystemProvider>.Success(provider);
        }

        return ResultType<IFileSystemProvider>.Fail($"Неизвестный режим файловой системы: {mode}");
    }
}