using Core.Entities;
using Core.Ports;
using Core.ResultInfo;

namespace Core.Services;

public class GetMoneyBalanceService : IGetMoneyBalanceService
{
    public IAccountRepository AccountRepository { get; private set; }

    public ISessionRepository SessionRepository { get; private set; }

    public GetMoneyBalanceService(IAccountRepository accountRepository, ISessionRepository sessionRepository)
    {
        AccountRepository = accountRepository;
        SessionRepository = sessionRepository;
    }

    public ResultType<MoneyState> Execute(Guid accountId, Guid sessionKey)
    {
        UserSession? session = SessionRepository.GetByKey(sessionKey);
        if (session == null)
        {
            return ResultType<MoneyState>.Fail("Сессия не найдена");
        }

        if (!session.IsAdmin && session.AccountId != accountId)
        {
            return ResultType<MoneyState>.Fail("Нет доступа к счёту");
        }

        Account? account = AccountRepository.GetById(accountId);
        if (account is null)
        {
            return ResultType<MoneyState>.Fail("Счёт не найден");
        }

        if (account.MoneyBalance is null)
        {
            return ResultType<MoneyState>.Fail("Баланс не определён");
        }

        return ResultType<MoneyState>.Success(account.MoneyBalance);
    }
}