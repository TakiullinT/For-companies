using Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab2.TopicInfo;

public interface ITopic
{
    Result SendMessage(Message message);
}