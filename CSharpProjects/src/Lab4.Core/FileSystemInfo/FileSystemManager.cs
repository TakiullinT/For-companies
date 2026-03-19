using Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Factories;
using Itmo.ObjectOrientedProgramming.Lab4.Core.PathServices;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Providers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;

public class FileSystemManager
{
    public FileSystemState State { get; private set; }

    private IFileSystemProvider? _activeProvider;

    public IFileSystemFactory Factory { get; private set; }

    public IPath Path { get; private set; }

    public FileSystemManager(IFileSystemFactory factory, IPath path)
    {
        Factory = factory;
        Path = path;
        State = new FileSystemState();
    }

    public ICommandResult DeleteFile(string path)
    {
        if (_activeProvider != null)
        {
            return ExecuteFileOperation(path, _activeProvider.DeleteFile);
        }

        return Result.Fail("Не удалось удалить файл");
    }

    public ICommandResult RenameFile(string path, string newName)
    {
        if (_activeProvider is not null)
        {
            return ExecuteFileOperation(path, absolutePath => _activeProvider.RenameFile(absolutePath, newName));
        }

        return Result.Fail("Ошибка переименовывания файла");
    }

    public ICommandResult? Connect(string address, string mode)
    {
        ICommandResult factoryResult = Factory.Create(mode);

        if (!factoryResult.IsSuccess || factoryResult is not ResultType<IFileSystemProvider> providerResult)
        {
            return factoryResult;
        }

        _activeProvider = providerResult.Value;
        ICommandResult? connectResult = _activeProvider?.Connect(address);

        if (connectResult != null && connectResult.IsSuccess)
        {
            State = new FileSystemState(address, address, mode);
            return Result.Success($"Успешно подключено к '{address}' в режиме '{mode}'");
        }

        _activeProvider = null;
        return connectResult;
    }

    public ICommandResult Disconnect()
    {
        if (!State.IsConnected || _activeProvider == null)
        {
            return Result.Fail("Система не подключена");
        }

        try
        {
            _activeProvider.Disconnect();
            _activeProvider = null;
            State = new FileSystemState();
            return Result.Success("Соединение разорвано");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка при отключении: {ex.Message}");
        }
    }

    public ICommandResult Goto(string targetPath)
    {
        if (!State.IsConnected)
        {
            return Result.Fail("Сначала необходимо подключиться к файловой системе");
        }

        ICommandResult resolveResult = Path.ResolvePath(State.ConnectionPath, State.LocalPath, targetPath);

        if (!resolveResult.IsSuccess)
        {
            return resolveResult;
        }

        string? newAbsolutePath = ((ResultType<string>)resolveResult).Value;

        if (newAbsolutePath != null && !Path.IsPathWithRoot(State.ConnectionPath, newAbsolutePath))
        {
            return Result.Fail($"Попытка выхода за пределы пути подключения: {State.ConnectionPath}");
        }

        try
        {
            if (newAbsolutePath != null) _activeProvider?.ListDirectory(newAbsolutePath);
        }
        catch (DirectoryNotFoundException)
        {
            return Result.Fail($"Путь '{targetPath}' не существует или не является каталогом");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка проверки пути: {ex.Message}");
        }

        State = State with { LocalPath = newAbsolutePath };
        return Result.Success($"Локальный путь изменен на: {newAbsolutePath}");
    }

    public DirectoryEntry TreeList(string path, int depth)
    {
        return new DirectoryEntry(Path.GetLastName(path), path, _activeProvider, depth);
    }

    public ICommandResult CopyFile(string sourcePath, string destinationPath)
    {
        if (!State.IsConnected || _activeProvider == null)
        {
            return Result.Fail("Система не подключена");
        }

        ICommandResult sourceResolvePath = Path.ResolvePath(State.ConnectionPath, State.LocalPath, sourcePath);
        if (!sourceResolvePath.IsSuccess || sourceResolvePath is not ResultType<string> sourcePathResult)
        {
            return sourceResolvePath;
        }

        if (sourcePathResult.Value == null)
        {
            return Result.Fail(string.Empty);
        }

        string absoluteSourcePath = sourcePathResult.Value;

        ICommandResult destinationResolvePath = Path.ResolvePath(State.ConnectionPath, State.LocalPath, destinationPath);
        if (!destinationResolvePath.IsSuccess || destinationResolvePath is not ResultType<string> destinationPathResult)
        {
            return destinationResolvePath;
        }

        string? absoluteDestinationPath = destinationPathResult.Value;
        if (!Path.IsPathWithRoot(State.ConnectionPath, absoluteDestinationPath))
        {
            return Result.Fail($"Целевой путь находится за пределами подключения: {State.ConnectionPath}");
        }

        try
        {
            return _activeProvider.Copy(absoluteSourcePath, absoluteDestinationPath);
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при копировании {exception.Message}");
        }
    }

    public ICommandResult FileShow(string path)
    {
        if (!State.IsConnected || _activeProvider == null)
        {
            return Result.Fail("Система не подключена");
        }

        ICommandResult resolveResult = Path.ResolvePath(State.ConnectionPath, State.LocalPath, path);

        if (!resolveResult.IsSuccess || resolveResult is not ResultType<string> pathResult)
        {
            return resolveResult;
        }

        if (pathResult.Value == null)
        {
            return Result.Fail(string.Empty);
        }

        string absolutePath = pathResult.Value;

        try
        {
            var fileEntry = new FileEntry(Path.GetLastName(absolutePath), absolutePath, _activeProvider);
            ICommandResult? result = fileEntry.GetContent(absolutePath);
            if (result == null)
            {
                return Result.Fail("Не удалось получить содержимое файла.");
            }

            return result;
        }
        catch (FileNotFoundException)
        {
            return Result.Fail($"Файл не найден по пути: {path}");
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при чтении файла : {exception.Message}");
        }
    }

    public ICommandResult FileMove(string sourcePath, string destinationPath)
    {
        if (!State.IsConnected || _activeProvider == null)
        {
            return Result.Fail("Система не подключена");
        }

        ICommandResult sourcePathResolveResult = Path.ResolvePath(State.ConnectionPath, State.LocalPath, sourcePath);
        if (!sourcePathResolveResult.IsSuccess || sourcePathResolveResult is not ResultType<string> sourcePathResult)
        {
            return sourcePathResolveResult;
        }

        if (sourcePathResult.Value == null)
        {
            return Result.Fail(string.Empty);
        }

        string absoluteSourcePath = sourcePathResult.Value;

        ICommandResult destinationPathResolveResult = Path.ResolvePath(State.ConnectionPath, State.LocalPath, destinationPath);
        if (!destinationPathResolveResult.IsSuccess ||
            destinationPathResolveResult is not ResultType<string> destinationPathResult)
        {
            return destinationPathResolveResult;
        }

        string? absoluteDestinationPath = destinationPathResult.Value;
        if (!Path.IsPathWithRoot(State.ConnectionPath, absoluteDestinationPath))
        {
            return Result.Fail($"Целевой путь находится за пределами подключения: {State.ConnectionPath}");
        }

        try
        {
            return _activeProvider.Move(absoluteSourcePath, absoluteDestinationPath);
        }
        catch (Exception exception)
        {
            return Result.Fail($"Ошибка при перемещении файла {exception.Message}");
        }
    }

    private ICommandResult ExecuteFileOperation(string sourcePath, Func<string, ICommandResult> operation)
    {
        if (!State.IsConnected)
        {
            return Result.Fail("Система не подключена");
        }

        if (State.LocalPath != null)
        {
            ICommandResult resolveResult = Path.ResolvePath(State.ConnectionPath, State.LocalPath, sourcePath);
            if (!resolveResult.IsSuccess)
            {
                return resolveResult;
            }

            string? absolutePath = ((ResultType<string>)resolveResult).Value;

            if (absolutePath != null)
            {
                return operation(absolutePath);
            }
        }

        return Result.Fail($"Невозможно выполнить операцию: текущий локальный путь отсутствует для {sourcePath}");
    }
}