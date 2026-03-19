using Core.Entities;
using Core.ResultInfo;

namespace Core.Ports;

public interface ICreateAdminSession
{
    ResultType<UserSession> Execute(string password);
}