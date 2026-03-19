using Core.Entities;

namespace Core.Ports;

public interface ISessionRepository
{
    UserSession? GetByKey(Guid sessionKey);

    void Save(UserSession userSession);
}