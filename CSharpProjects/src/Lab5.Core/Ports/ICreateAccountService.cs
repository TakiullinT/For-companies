using Core.Entities;
using Core.ResultInfo;

namespace Core.Ports;

public interface ICreateAccountService
{
    ResultType<Account> Execute(Guid sessionKey, string pin);
}