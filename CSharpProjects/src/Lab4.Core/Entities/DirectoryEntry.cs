using Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;

public class DirectoryEntry : FileSystemEntry
{
    private IEnumerable<FileSystemEntry>? _children;

    public int RemainingDepth { get; private set; }

    public DirectoryEntry(string name, string path, IFileSystemProvider? provider, int remainingDepth) : base(
        name,
        path,
        provider)
    {
        RemainingDepth = remainingDepth;
    }

    public IEnumerable<FileSystemEntry>? GetChildren()
    {
        if (RemainingDepth <= 0 || Provider == null)
        {
            return Enumerable.Empty<FileSystemEntry>();
        }

        if (_children != null)
        {
            return _children;
        }

        IEnumerable<FileSystemEntry> providerEntries;
        try
        {
            providerEntries = Provider.ListDirectory(AbsolutePath);
        }
        catch
        {
            _children = Enumerable.Empty<FileSystemEntry>();
            return _children;
        }

        if (providerEntries == null)
        {
            _children = Enumerable.Empty<FileSystemEntry>();
            return _children;
        }

        var resolvedEntries = new List<FileSystemEntry>();

        foreach (FileSystemEntry item in providerEntries)
        {
            if (item is DirectoryEntry)
            {
                resolvedEntries.Add(new DirectoryEntry(item.Name, item.AbsolutePath, Provider, RemainingDepth - 1));
            }
            else if (item is FileEntry)
            {
                resolvedEntries.Add(new FileEntry(item.Name, item.AbsolutePath, Provider));
            }
            else
            {
                resolvedEntries.Add(item);
            }
        }

        _children = resolvedEntries;
        return _children;
    }
}