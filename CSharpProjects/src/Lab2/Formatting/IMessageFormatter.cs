using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Formatting;

public interface IMessageFormatter
{
    Result WriteHeader(string header);

    Result WriteBody(string body);

    Result WriteImportance(Message.Importance importance);

    string GetFormattedMessage();
}