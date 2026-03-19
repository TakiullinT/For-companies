using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.Archiving;

public interface IMessageArchiver
{
    Result SaveMessage(Message message);
}