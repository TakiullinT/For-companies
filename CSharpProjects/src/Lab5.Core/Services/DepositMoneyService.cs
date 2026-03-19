using Core.Entities;
using Core.Ports;
using Core.ResultInfo;

namespace Core.Services;

public class DepositMoneyService : IDepositMoneyService
{
    public ISessionRepository SessionRepository { get; private set; }

    public IAccountRepository AccountRepository { get; private set; }

    public DepositMoneyService(ISessionRepository sessionRepository, IAccountRepository accountRepository)
    {
        SessionRepository = sessionRepository;
        AccountRepository = accountRepository;
    }

    public Result Execute(Guid sessionKey, Guid accountId, MoneyState amount)
    {
        if (amount == null) return Result.Fail("Сумма не задана");
        if (amount.Amount <= 0) return Result.Fail("Сумма должна быть больше нуля");

        UserSession? session = SessionRepository.GetByKey(sessionKey);
        if (session == null) return Result.Fail("Сессия не найдена");

        if (!session.IsAdmin && session.AccountId != accountId)
        {
            return Result.Fail("Нет доступа к счёту");
        }

        Account? account = AccountRepository.GetById(accountId);
        if (account == null) return Result.Fail("Счёт не найден");

        account.Deposit(amount);
        AccountRepository.Save(account);
        return Result.Success();
    }
}