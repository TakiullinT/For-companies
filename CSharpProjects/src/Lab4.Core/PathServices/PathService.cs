using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.PathServices;

public class PathService : IPath
{
    private const char Separator = '/';

    public ICommandResult ResolvePath(string rootPath, string? currentPath, string targetPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            return Result.Fail("Корень не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(targetPath))
        {
            return Result.Fail("Целевой путь не задан");
        }

        bool isTargetAbsolute = targetPath.StartsWith(Separator);
        string basePath;
        string cleanRoot = rootPath.TrimEnd(Separator);

        if (isTargetAbsolute)
        {
            if (targetPath.StartsWith(cleanRoot, StringComparison.Ordinal))
            {
                basePath = targetPath;
            }
            else
            {
                basePath = cleanRoot + targetPath;
            }
        }
        else
        {
            string contextPath = currentPath ?? cleanRoot;
            basePath = contextPath.TrimEnd(Separator) + Separator + targetPath;
        }

        string resolvedPath = Normalize(basePath);
        if (!IsPathWithRoot(rootPath, resolvedPath))
        {
            return ResultType<string>.Fail("Ошибка: Путь выходит за пределы корневого каталога подключения");
        }

        return ResultType<string>.Success(resolvedPath);
    }

    public bool IsPathWithRoot(string rootPath, string? absolutePath)
    {
        string cleanRoot = rootPath.TrimEnd(Separator);
        if (absolutePath == null)
        {
            return false;
        }

        string normalizedPath = absolutePath.TrimEnd(Separator);

        return normalizedPath.Equals(cleanRoot, StringComparison.Ordinal) ||
               normalizedPath.StartsWith(cleanRoot + Separator, StringComparison.Ordinal);
    }

    public string GetLastName(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        string cleanedPath = path.TrimEnd(Separator);
        string[] parts = cleanedPath.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            return Separator.ToString();
        }

        return parts.Last();
    }

    private string Normalize(string path)
    {
        string[] parts = path.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        var stack = new Stack<string>();

        foreach (string part in parts)
        {
            if (part == ".")
            {
                continue;
            }

            if (part == "..")
            {
                if (stack.Count > 0)
                {
                    stack.Pop();
                }

                continue;
            }

            stack.Push(part);
        }

        return Separator + string.Join(Separator, stack.Reverse());
    }
}