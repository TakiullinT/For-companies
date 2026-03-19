using Core.Entities;
using Core.Ports;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public class MockSessionRepository : ISessionRepository
{
    private readonly Dictionary<Guid, UserSession> _sessions
        = new();

    public UserSession? GetByKey(Guid sessionKey)
    {
        _sessions.TryGetValue(sessionKey, out UserSession? session);
        return session;
    }

    public void Save(UserSession userSession)
    {
        _sessions[userSession.SessionKey] = userSession;
    }

    public void Add(UserSession session) =>
        _sessions[session.SessionKey] = session;
}

