using Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;
using DirectoryEntry = Itmo.ObjectOrientedProgramming.Lab4.Core.Entities.DirectoryEntry;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;

public class LocalFileSystemProvider : IFileSystemProvider
{
    public ICommandResult Connect(string address)
    {
        try
        {
            if (Directory.Exists(address))
            {
                return Result.Success("Успешное подключение к локальному пути");
            }

            return Result.Fail($"Каталог не найден по адресу: {address}");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при попытке подключения: {exception.Message}");
        }
    }

    public void Disconnect()
    {
    }

    public IEnumerable<FileSystemEntry> ListDirectory(string? path)
    {
        if (!Directory.Exists(path) || string.IsNullOrEmpty(path))
        {
            return Enumerable.Empty<FileSystemEntry>();
        }

        IEnumerable<DirectoryEntry> directories = Directory.EnumerateDirectories(path)
            .Select(directory => new DirectoryEntry(Path.GetFileName(directory), directory, this, 0));

        IEnumerable<FileEntry> files = Directory.EnumerateFiles(path)
            .Select(file => new FileEntry(Path.GetFileName(file), file, this));

        return directories.Cast<FileSystemEntry>().Concat(files);
    }

    public ICommandResult ReadFileContent(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                return Result.Fail($"Файл не найден: {path}");
            }

            string content = File.ReadAllText(path);
            return Result.Success(content);
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при чтении файла: {exception.Message}");
        }
    }

    public ICommandResult DeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return Result.Success("Файл успешно удалён");
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return Result.Success("Каталог успешно удалён");
            }

            return Result.Fail("Элемент не найден");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при удалении {exception.Message}");
        }
    }

    public ICommandResult RenameFile(string path, string newName)
    {
        try
        {
            if (File.Exists(path))
            {
                string? directoryName = Path.GetDirectoryName(path) ?? string.Empty;
                string newPath = Path.Combine(directoryName, newName);

                if (string.IsNullOrEmpty(directoryName))
                {
                    return Result.Fail("Невозможно определить каталог для переименования");
                }

                if (File.Exists(newPath) || Directory.Exists(newPath))
                {
                    return Result.Fail("Целевая сущность уже существует");
                }

                File.Move(path, newPath);
                return Result.Success("Файл переименован успешно");
            }

            if (Directory.Exists(path))
            {
                string parentDirectoryName = Path.GetDirectoryName(path) ?? string.Empty;
                string newPath = Path.Combine(parentDirectoryName, newName);
                if (Directory.Exists(newPath))
                {
                    return Result.Fail("Целевая сущность уже существует");
                }

                Directory.Move(path, newPath);
                return Result.Success("Каталог переименован успешно");
            }

            return Result.Fail("Элемент не найден");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при переименовании: {exception.Message}");
        }
    }

    public ICommandResult Move(string sourcePath, string? destinationPath)
    {
        try
        {
            if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
            {
                return Result.Fail("Исходный элемент не найден");
            }

            if (!Directory.Exists(destinationPath))
            {
                return Result.Fail("Целевая директория не найдена");
            }

            string destinationFullPath = Path.Combine(destinationPath, Path.GetFileName(sourcePath));
            if (File.Exists(destinationFullPath) || Directory.Exists(destinationFullPath))
            {
                return Result.Fail("Элемент с таким именем уже существует в целевой директории");
            }

            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, destinationFullPath);
            }
            else
            {
                Directory.Move(sourcePath, destinationFullPath);
            }

            return Result.Success($"Перемещено из {sourcePath} в {destinationFullPath}");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при перемещении: {exception.Message}");
        }
    }

    public ICommandResult Copy(string? sourcePath, string? destinationPath)
    {
        try
        {
            if (!File.Exists(sourcePath) || string.IsNullOrWhiteSpace(sourcePath))
            {
                return Result.Fail("Исходный файл не найден");
            }

            if (string.IsNullOrEmpty(destinationPath))
            {
                return Result.Fail("Целевая директория не указана");
            }

            string destinationFullPath = Path.Combine(destinationPath, Path.GetFileName(sourcePath));
            if (File.Exists(destinationFullPath) || Directory.Exists(destinationFullPath))
            {
                return Result.Fail("Файл с таким именем уже существует в целевой директории");
            }

            File.Copy(sourcePath, destinationFullPath);
            return Result.Success($"Скопировано в {destinationFullPath}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка при копировании: {ex.Message}");
        }
    }
}