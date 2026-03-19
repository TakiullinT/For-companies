using Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;

public interface IFileSystemProvider
{
    ICommandResult Connect(string address);

    void Disconnect();

    ICommandResult DeleteFile(string path);

    ICommandResult RenameFile(string path, string newName);

    ICommandResult Move(string sourcePath, string? destinationPath);

    ICommandResult Copy(string? sourcePath, string? destinationPath);

    IEnumerable<FileSystemEntry> ListDirectory(string? path);

    ICommandResult ReadFileContent(string path);
}