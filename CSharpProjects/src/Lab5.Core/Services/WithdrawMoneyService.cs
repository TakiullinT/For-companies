using Core.Entities;
using Core.Ports;
using Core.ResultInfo;

namespace Core.Services;

public class WithdrawMoneyService : IWithdrawMoneyService
{
    public IAccountRepository AccountRepository { get; private set; }

    public ISessionRepository SessionRepository { get; private set; }

    public WithdrawMoneyService(IAccountRepository accountRepository, ISessionRepository sessionRepository)
    {
        AccountRepository = accountRepository;
        SessionRepository = sessionRepository;
    }

    public Result Execute(Guid sessionKey, Guid accountId, MoneyState amount)
    {
        if (amount is null) return Result.Fail("Сумма не задана!");
        if (amount.Amount <= 0) return Result.Fail("Сумма должна быть больше нуля!");

        UserSession? session = SessionRepository.GetByKey(sessionKey);
        if (session is null) return Result.Fail("Сессия не найдена!");

        if (!session.IsAdmin && session.AccountId != accountId)
        {
            return Result.Fail("Нет доступа к счёту!");
        }

        Account? account = AccountRepository.GetById(accountId);
        if (account is null) return Result.Fail("Счёт не найден!");

        Result withdrawResult = account.Withdraw(amount);
        if (!withdrawResult.IsSuccess) return withdrawResult;

        AccountRepository.Save(account);
        return Result.Success();
    }
}