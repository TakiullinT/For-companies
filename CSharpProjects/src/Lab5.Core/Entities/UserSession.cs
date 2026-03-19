namespace Core.Entities;

public class UserSession
{
    public Guid SessionKey { get; private set; }

    public Guid? AccountId { get; private set; }

    public bool IsAdmin { get; private set; }

    private UserSession() { }

    public static UserSession CreateUserSession(Guid accountId)
    {
        return new UserSession
        {
            SessionKey = Guid.NewGuid(),
            AccountId = accountId,
            IsAdmin = false,
        };
    }

    public static UserSession CreateAdminSession()
    {
        return new UserSession
        {
            SessionKey = Guid.NewGuid(),
            AccountId = null,
            IsAdmin = true,
        };
    }
}