using Core.Entities;
using Core.Ports;
using Core.ResultInfo;
using Microsoft.Extensions.Options;

namespace Core.Services;

public class CreateAdminSession : ICreateAdminSession
{
    public ISessionRepository? SessionRepository { get; private set; }

    public string SystemPassword { get; private set; }

    public CreateAdminSession(ISessionRepository sessionRepository, IOptions<AdminOptions> options)
    {
        SessionRepository = sessionRepository;
        SystemPassword = options.Value.SystemPassword;
    }

    public ResultType<UserSession> Execute(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password != SystemPassword)
        {
            return ResultType<UserSession>.Fail("Пароли не совпадают!");
        }

        var session = UserSession.CreateAdminSession();
        SessionRepository?.Save(session);
        return ResultType<UserSession>.Success(session);
    }
}