using Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;

public abstract class FileSystemEntry
{
    public string Name { get; }

    public string AbsolutePath { get; }

    protected IFileSystemProvider? Provider { get; }

    protected FileSystemEntry(string name, string absolutePath, IFileSystemProvider? provider)
    {
        Name = name;
        AbsolutePath = absolutePath;
        Provider = provider;
    }
}