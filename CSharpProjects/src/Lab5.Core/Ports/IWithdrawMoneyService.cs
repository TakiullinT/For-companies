using Core.Entities;
using Core.ResultInfo;

namespace Core.Ports;

public interface IWithdrawMoneyService
{
    Result Execute(Guid sessionKey, Guid accountId, MoneyState amount);
}