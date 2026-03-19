using Core.Entities;
using Core.ResultInfo;

namespace Core.Ports;

public interface IGetMoneyBalanceService
{
    ResultType<MoneyState> Execute(Guid accountId, Guid sessionKey);
}