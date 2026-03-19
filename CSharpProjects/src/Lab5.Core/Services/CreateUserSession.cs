using Core.Entities;
using Core.Ports;
using Core.ResultInfo;

namespace Core.Services;

public class CreateUserSession : ICreateUserSession
{
    public ISessionRepository SessionRepository { get; private set; }

    public IAccountRepository AccountRepository { get; private set; }

    public CreateUserSession(ISessionRepository sessionRepository, IAccountRepository accountRepository)
    {
        SessionRepository = sessionRepository;
        AccountRepository = accountRepository;
    }

    public ResultType<UserSession> Execute(Guid accountId, string pin)
    {
        Account? account = AccountRepository.GetById(accountId);
        if (account is null)
        {
            return ResultType<UserSession>.Fail("Счёт не найден!");
        }

        if (account.Pin != pin)
        {
            return ResultType<UserSession>.Fail("ПИН-коды не совпадают!");
        }

        var session = UserSession.CreateUserSession(accountId);
        SessionRepository.Save(session);
        return ResultType<UserSession>.Success(session);
    }
}