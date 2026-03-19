using Core.Entities;
using Core.ResultInfo;

namespace Core.Ports;

public interface ICreateUserSession
{
    ResultType<UserSession> Execute(Guid accountId, string pin);
}