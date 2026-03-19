using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Entities;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class ConsoleRenderer
{
    public void RenderResult(ICommandResult commandResult)
    {
        Console.ForegroundColor = commandResult.IsSuccess ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(commandResult.Message);

        if (commandResult.IsSuccess && commandResult is ResultType<DirectoryEntry> directoryResult)
        {
            if (directoryResult.Value is not null)
            {
                RenderDirectoryTree(directoryResult.Value);
            }
        }
        else if (commandResult.IsSuccess && commandResult is ResultType<string> fileContentResult &&
                 commandResult is not ResultType<ICommand>)
        {
            Console.WriteLine("--- Содержимое файла ---");
            Console.WriteLine(fileContentResult.Value);
            Console.WriteLine("-----------------------");
        }

        Console.ResetColor();
    }

    public void RenderDirectoryTree(DirectoryEntry rootDirectory, string prefix = "")
    {
        if (rootDirectory.GetChildren() == null) return;

        var children = (rootDirectory.GetChildren() ?? Array.Empty<FileSystemEntry>()).ToList();
        for (int i = 0; i < children.Count; i++)
        {
            FileSystemEntry entry = children[i];
            bool isLast = i == children.Count - 1;

            string branchSymbol = isLast ? "└── " : "├── ";

            Console.Write(prefix);
            Console.Write(branchSymbol);

            if (entry is DirectoryEntry directory)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(entry.Name + "/");
                Console.ResetColor();

                string newPrefix = prefix + (isLast ? "    " : "│   ");
                RenderDirectoryTree(directory, newPrefix);
            }
            else
            {
                Console.WriteLine(entry.Name);
            }
        }
    }
}