namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystemInfo;

public record FileSystemState
{
    public string ConnectionPath { get; private set; }

    public string? LocalPath { get; init; }

    public string ActiveMode { get; private set; }

    public bool IsConnected { get; private set; }

    public FileSystemState()
    {
        ConnectionPath = string.Empty;
        LocalPath = string.Empty;
        ActiveMode = string.Empty;
        IsConnected = false;
    }

    public FileSystemState(string connectionPath, string localPath, string activeMode, bool isConnected = true)
    {
        ConnectionPath = connectionPath;
        LocalPath = localPath;
        ActiveMode = activeMode;
        IsConnected = isConnected;
    }
}