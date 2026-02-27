namespace Timescale.Api.Domain.Exceptions;

public class InvalidFileException : DomainException
{
    public IEnumerable<string> Errors { get; }

    public InvalidFileException(string message) : base(message)
    {
        Errors = new[] { message };
    }

    public InvalidFileException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}