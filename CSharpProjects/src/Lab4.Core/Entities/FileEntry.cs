using Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;

public class FileEntry : FileSystemEntry
{
    public FileEntry(string name, string path, IFileSystemProvider provider) : base(name, path, provider) { }

    public ICommandResult? GetContent(string path)
    {
        return Provider?.ReadFileContent(path);
    }
}