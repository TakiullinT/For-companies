using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.PathServices;

public interface IPath
{
    ICommandResult ResolvePath(string rootPath, string? currentPath, string targetPath);

    bool IsPathWithRoot(string rootPath, string? absolutePath);

    string GetLastName(string path);
}