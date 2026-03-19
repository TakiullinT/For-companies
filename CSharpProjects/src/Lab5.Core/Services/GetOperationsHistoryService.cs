using Core.Entities;
using Core.Ports;
using Core.ResultInfo;

namespace Core.Services;

public class GetOperationsHistoryService : IGetOperationHistoryService
{
    public IAccountRepository AccountRepository { get; private set; }

    public ISessionRepository SessionRepository { get; private set; }

    public GetOperationsHistoryService(IAccountRepository accountRepository, ISessionRepository sessionRepository)
    {
        AccountRepository = accountRepository;
        SessionRepository = sessionRepository;
    }

    public ResultType<IReadOnlyCollection<Operation>> Execute(Guid accountId, Guid sessionKey)
    {
        UserSession? session = SessionRepository.GetByKey(sessionKey);
        if (session is null)
        {
            return ResultType<IReadOnlyCollection<Operation>>.Fail("Сессия не найдена!");
        }

        if (!session.IsAdmin && session.AccountId != accountId)
        {
            return ResultType<IReadOnlyCollection<Operation>>.Fail("Нет доступа к истории операций!");
        }

        Account? account = AccountRepository.GetById(accountId);
        if (account is null)
        {
            return ResultType<IReadOnlyCollection<Operation>>.Fail("Счёт не найден!");
        }

        return ResultType<IReadOnlyCollection<Operation>>.Success(account.OperationsHistory);
    }
}