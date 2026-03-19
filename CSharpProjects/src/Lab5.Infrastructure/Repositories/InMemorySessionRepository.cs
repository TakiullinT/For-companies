using Core.Entities;
using Core.Ports;
using System.Collections.Concurrent;

namespace Lab5.Infrastructure.Repositories;

public class InMemorySessionRepository : ISessionRepository
{
    public ConcurrentDictionary<Guid, UserSession> Sessions { get; private set; } = new();

    public UserSession? GetByKey(Guid sessionKey)
    {
        Sessions.TryGetValue(sessionKey, out UserSession? session);
        return session;
    }

    public void Save(UserSession userSession)
    {
        Sessions[userSession.SessionKey] = userSession;
    }
}