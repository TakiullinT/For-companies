using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab4.Tests;

public class ParsersTests
{
    [Fact]
    public void ConnectParser_WithModeFlag_ArgsValid_CanParseReturnsTrue()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba", "-m", "local" };

        Assert.True(parser.CanParse(args));
    }

    [Fact]
    public void ConnectParser_WithModeFlag_WhenParsed_CommandIsConnectCommand()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba", "-m", "local" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as ConnectCommand;
        Assert.NotNull(command);
    }

    [Fact]
    public void ConnectParser_WithModeFlag_WhenParsed_ParsesAddress()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba", "-m", "local" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as ConnectCommand;
        Assert.NotNull(command);
        Assert.Equal("/Users/macbook/TestLaba", command.Address);
    }

    [Fact]
    public void ConnectParser_WithModeFlag_WhenParsed_ParsesAddressAndMode()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba", "-m", "local" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as ConnectCommand;
        Assert.NotNull(command);
        Assert.Equal("/Users/macbook/TestLaba", command.Address);
        Assert.Equal("local", command.Mode);
    }

    [Fact]
    public void ConnectParser_WithoutModeFlag_ArgsValid_CanParseReturnsTrue()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba" };

        Assert.True(parser.CanParse(args));
    }

    [Fact]
    public void ConnectParser_WithoutModeFlag_WhenParsed_CommandIsConnectCommand()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as ConnectCommand;
        Assert.NotNull(command);
    }

    [Fact]
    public void ConnectParser_WithoutModeFlag_WhenParsed_ParsesAddress()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as ConnectCommand;
        Assert.NotNull(command);
        Assert.Equal("/Users/macbook/TestLaba", command.Address);
    }

    [Fact]
    public void ConnectParser_WithoutModeFlag_WhenParsed_ParsesAddressAndDefaultMode()
    {
        var parser = new ConnectParser();
        string[] args = new[] { "connect", "/Users/macbook/TestLaba" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as ConnectCommand;
        Assert.NotNull(command);
        Assert.Equal("/Users/macbook/TestLaba", command.Address);
        Assert.Equal("local", command.Mode);
    }

    [Theory]
    [InlineData("copy", "a.txt", "b/")]
    [InlineData("file", "copy", "a.txt", "b/")]
    public void CopyParser_ValidArgs_CanParseReturnsTrue(params string[] args)
    {
        var parser = new CopyParser();

        Assert.True(parser.CanParse(args));
    }

    [Theory]
    [InlineData("copy", "a.txt", "b/")]
    [InlineData("file", "copy", "a.txt", "b/")]
    public void CopyParser_ValidArgs_CommandIsCopyCommand(params string[] args)
    {
        var parser = new CopyParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileCopyCommand;
        Assert.NotNull(command);
    }

    [Theory]
    [InlineData("copy", "a.txt", "b/")]
    [InlineData("file", "copy", "a.txt", "b/")]
    public void CopyParser_ValidArgs_ParsesSource(params string[] args)
    {
        var parser = new CopyParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileCopyCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.SourcePath);
    }

    [Theory]
    [InlineData("copy", "a.txt", "b/")]
    [InlineData("file", "copy", "a.txt", "b/")]
    public void CopyParser_ValidArgs_ParsesSourceAndDestination(params string[] args)
    {
        var parser = new CopyParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileCopyCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.SourcePath);
        Assert.Equal("b/", command.DestinationPath);
    }

    [Theory]
    [InlineData("move", "a.txt", "dest/")]
    [InlineData("file", "move", "a.txt", "dest/")]
    public void MoveParser_ValidArgs_CanParseReturnsTrue(params string[] args)
    {
        var parser = new MoveParser();

        Assert.True(parser.CanParse(args));
    }

    [Theory]
    [InlineData("move", "a.txt", "dest/")]
    [InlineData("file", "move", "a.txt", "dest/")]
    public void MoveParser_ValidArgs_CommandIsMoveCommand(params string[] args)
    {
        var parser = new MoveParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileMoveCommand;
        Assert.NotNull(command);
    }

    [Theory]
    [InlineData("move", "a.txt", "dest/")]
    [InlineData("file", "move", "a.txt", "dest/")]
    public void MoveParser_ValidArgs_ParsesSource(params string[] args)
    {
        var parser = new MoveParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileMoveCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.SourcePath);
    }

    [Theory]
    [InlineData("move", "a.txt", "dest/")]
    [InlineData("file", "move", "a.txt", "dest/")]
    public void MoveParser_ValidArgs_ParsesSourceAndDestination(params string[] args)
    {
        var parser = new MoveParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileMoveCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.SourcePath);
        Assert.Equal("dest/", command.DestinationPath);
    }

    [Theory]
    [InlineData("delete", "a.txt")]
    [InlineData("file", "delete", "a.txt")]
    public void DeleteParser_ValidArgs_CanParseReturnsTrue(params string[] args)
    {
        var parser = new DeleteParser();

        Assert.True(parser.CanParse(args));
    }

    [Theory]
    [InlineData("delete", "a.txt")]
    [InlineData("file", "delete", "a.txt")]
    public void DeleteParser_ValidArgs_CommandIsDeleteCommand(params string[] args)
    {
        var parser = new DeleteParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileDeleteCommand;
        Assert.NotNull(command);
    }

    [Theory]
    [InlineData("delete", "a.txt")]
    [InlineData("file", "delete", "a.txt")]
    public void DeleteParser_ValidArgs_ParsesPathCorrectly(params string[] args)
    {
        var parser = new DeleteParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileDeleteCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.Path);
    }

    [Theory]
    [InlineData("rename", "a.txt", "b.txt")]
    [InlineData("file", "rename", "a.txt", "b.txt")]
    public void RenameParser_ValidArgs_CanParseReturnsTrue(params string[] args)
    {
        var parser = new RenameParser();

        Assert.True(parser.CanParse(args));
    }

    [Theory]
    [InlineData("rename", "a.txt", "b.txt")]
    [InlineData("file", "rename", "a.txt", "b.txt")]
    public void RenameParser_ValidArgs_CommandIsRenameCommand(params string[] args)
    {
        var parser = new RenameParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileRenameCommand;
        Assert.NotNull(command);
    }

    [Theory]
    [InlineData("rename", "a.txt", "b.txt")]
    [InlineData("file", "rename", "a.txt", "b.txt")]
    public void RenameParser_ValidArgs_ParsesOldName(params string[] args)
    {
        var parser = new RenameParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileRenameCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.Path);
    }

    [Theory]
    [InlineData("rename", "a.txt", "b.txt")]
    [InlineData("file", "rename", "a.txt", "b.txt")]
    public void RenameParser_ValidArgs_ParsesOldAndNewNames(params string[] args)
    {
        var parser = new RenameParser();

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileRenameCommand;
        Assert.NotNull(command);
        Assert.Equal("a.txt", command.Path);
        Assert.Equal("b.txt", command.NewName);
    }

    [Fact]
    public void ShowParser_WithModeFlag_CanParseReturnsTrue()
    {
        var parser = new ShowParser();
        string[] args = new[] { "file", "show", "notes.txt", "-m", "console" };

        Assert.True(parser.CanParse(args));
    }

    [Fact]
    public void ShowParser_WithModeFlag_CommandIsShowCommand()
    {
        var parser = new ShowParser();
        string[] args = new[] { "file", "show", "notes.txt", "-m", "console" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileShowCommand;
        Assert.NotNull(command);
    }

    [Fact]
    public void ShowParser_WithModeFlag_ParsesPathCorrectly()
    {
        var parser = new ShowParser();
        string[] args = new[] { "file", "show", "notes.txt", "-m", "console" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as FileShowCommand;
        Assert.NotNull(command);
        Assert.Equal("notes.txt", command.Path);
    }

    [Fact]
    public void ShowParser_WithoutModeFlag_CanParseReturnsFalseOrFailure()
    {
        var parser = new ShowParser();
        string[] args = new[] { "file", "show", "notes.txt" };

        ICommandResult result = parser.Parse(args);
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData("tree", "goto", "subdir")]
    public void TreeGoToParser_ValidArgs_CanParseReturnsTrue(params string[] args)
    {
        var parser = new TreeGoToParser();
        Assert.True(parser.CanParse(args));
    }

    [Theory]
    [InlineData("tree", "goto", "subdir")]
    public void TreeGoToParser_ValidArgs_CommandIsGoToCommand(params string[] args)
    {
        var parser = new TreeGoToParser();
        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeGoToCommand;
        Assert.NotNull(command);
    }

    [Theory]
    [InlineData("tree", "goto", "subdir")]
    public void TreeGoToParser_ValidArgs_ParsesTargetPath(params string[] args)
    {
        var parser = new TreeGoToParser();
        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeGoToCommand;
        Assert.NotNull(command);
        Assert.Equal("subdir", command.Path);
    }

    [Fact]
    public void TreeListParser_WithDepthFlag_CanParseReturnsTrue()
    {
        var parser = new TreeListParser();
        string[] args = new[] { "tree", "list", "-d", "2" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeListCommand;
        Assert.NotNull(command);
        Assert.Equal(2, command.Depth);
    }

    [Fact]
    public void TreeListParser_WithDepthFlag_CommandIsTreeListCommand()
    {
        var parser = new TreeListParser();
        string[] args = new[] { "tree", "list", "-d", "2" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeListCommand;
        Assert.NotNull(command);
        Assert.Equal(2, command.Depth);
    }

    [Fact]
    public void TreeListParser_WithDepthFlag_ParsesDepthCorrectly()
    {
        var parser = new TreeListParser();
        string[] args = new[] { "tree", "list", "-d", "2" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeListCommand;
        Assert.NotNull(command);
        Assert.Equal(2, command.Depth);
    }

    [Fact]
    public void TreeListParser_WithoutDepthFlag_CanParseReturnsTrue()
    {
        var parser = new TreeListParser();
        string[] args = new[] { "tree", "list" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeListCommand;
        Assert.NotNull(command);
        Assert.Equal(1, command.Depth);
    }

    [Fact]
    public void TreeListParser_WithoutDepthFlag_CommandIsTreeListCommand()
    {
        var parser = new TreeListParser();
        string[] args = new[] { "tree", "list" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeListCommand;
        Assert.NotNull(command);
        Assert.Equal(1, command.Depth);
    }

    [Fact]
    public void TreeListParser_WithoutDepthFlag_UsesDefaultDepth()
    {
        var parser = new TreeListParser();
        string[] args = new[] { "tree", "list" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as TreeListCommand;
        Assert.NotNull(command);
        Assert.Equal(1, command.Depth);
    }

    [Fact]
    public void DisconnectParser_ValidArgs_CanParseReturnsTrue()
    {
        var parser = new DisconnectParser();
        string[] args = new[] { "disconnect" };

        Assert.True(parser.CanParse(args));
    }

    [Fact]
    public void DisconnectParser_ValidArgs_CommandIsDisconnectCommand()
    {
        var parser = new DisconnectParser();
        string[] args = new[] { "disconnect" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        var command = commandResult.Value as DisconnectCommand;
        Assert.NotNull(command);
    }

    [Fact]
    public void DisconnectParser_ValidArgs_ReturnsFullyParsedCommand()
    {
        var parser = new DisconnectParser();
        string[] args = new[] { "disconnect" };

        Assert.True(parser.CanParse(args));
        ICommandResult result = parser.Parse(args);
        ResultType<ICommand> commandResult = AsCommandResult(result);
        Assert.IsType<ResultType<ICommand>>(result);
        var command = commandResult.Value as DisconnectCommand;
        Assert.NotNull(command);
    }

    [Fact]
    public void RootParser_CopyArgs_CanParseCopyCommand()
    {
        var parsers = new List<ICommandParser>
        {
            new ConnectParser(),
            new CopyParser(),
            new MoveParser(),
            new RenameParser(),
            new DeleteParser(),
            new DisconnectParser(),
            new ShowParser(),
            new TreeGoToParser(),
            new TreeListParser(),
        };

        var root = new RootParser(parsers);

        string[] args1 = new[] { "copy", "x.txt", "y/" };
        ICommandResult r1 = root.Parse(args1);
        Assert.True(r1.IsSuccess);
    }

    [Fact]
    public void RootParser_CopyArgs_CanParseDisconnectCommand()
    {
        var parsers = new List<ICommandParser>
        {
            new ConnectParser(),
            new CopyParser(),
            new MoveParser(),
            new RenameParser(),
            new DeleteParser(),
            new DisconnectParser(),
            new ShowParser(),
            new TreeGoToParser(),
            new TreeListParser(),
        };

        var root = new RootParser(parsers);

        string[] args1 = new[] { "copy", "x.txt", "y/" };
        ICommandResult r1 = root.Parse(args1);
        Assert.True(r1.IsSuccess);

        string[] args2 = new[] { "disconnect" };
        ICommandResult r2 = root.Parse(args2);
        Assert.True(r2.IsSuccess);
    }

    [Fact]
    public void RootParser_CopyArgs_CanParseTreeListtCommand()
    {
        var parsers = new List<ICommandParser>
        {
            new ConnectParser(),
            new CopyParser(),
            new MoveParser(),
            new RenameParser(),
            new DeleteParser(),
            new DisconnectParser(),
            new ShowParser(),
            new TreeGoToParser(),
            new TreeListParser(),
        };

        var root = new RootParser(parsers);

        string[] args1 = new[] { "copy", "x.txt", "y/" };
        ICommandResult r1 = root.Parse(args1);
        Assert.True(r1.IsSuccess);

        string[] args2 = new[] { "disconnect" };
        ICommandResult r2 = root.Parse(args2);
        Assert.True(r2.IsSuccess);

        string[] args3 = new[] { "tree", "list", "-d", "3" };
        ICommandResult r3 = root.Parse(args3);
        Assert.True(r3.IsSuccess);
    }

    [Fact]
    public void CopyParser_InvalidArgs_ReturnsFailure()
    {
        string[] args = new[] { "copy", "onlyOneArg" };
        var copyParser = new CopyParser();
        ICommandResult result = copyParser.Parse(args);
        Assert.False(result.IsSuccess);
    }

    private static ResultType<ICommand> AsCommandResult(ICommandResult result)
    {
        Assert.True(result.IsSuccess, $"Парсер вернул ошибку: {result.Message}");
        Assert.IsType<ResultType<ICommand>>(result);
        return (ResultType<ICommand>)result;
    }
}
