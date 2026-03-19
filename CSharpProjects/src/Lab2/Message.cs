namespace Itmo.ObjectOrientedProgramming.Lab2;

public class Message
{
    public enum Importance
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3,
    }

    public string Header { get; }

    public string Body { get; }

    public Importance Level { get; }

    public Message(string header, string body, Importance level = Importance.Medium)
    {
        Header = header;
        Body = body;
        Level = level;
    }
}