using Core.Entities;
using Core.ResultInfo;

namespace Core.Ports;

public interface IGetOperationHistoryService
{
    ResultType<IReadOnlyCollection<Operation>> Execute(Guid accountId, Guid sessionKey);
}