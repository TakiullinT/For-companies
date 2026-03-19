using Core.Entities;
using Core.Ports;
using Core.ResultInfo;

namespace Core.Services;

public class CreateAccountService : ICreateAccountService
{
    public IAccountRepository AccountRepository { get; private set; }

    public ISessionRepository SessionRepository { get; private set; }

    public CreateAccountService(IAccountRepository accountRepository, ISessionRepository sessionRepository)
    {
        AccountRepository = accountRepository;
        SessionRepository = sessionRepository;
    }

    public ResultType<Account> Execute(Guid sessionKey, string pin)
    {
        UserSession? session = SessionRepository.GetByKey(sessionKey);
        if (session is null) return ResultType<Account>.Fail("Сессия не найдена!");
        if (!session.IsAdmin) return ResultType<Account>.Fail("Нет прав администратора!");

        var account = new Account(Guid.NewGuid(), pin);
        AccountRepository.Save(account);
        return ResultType<Account>.Success(account);
    }
}