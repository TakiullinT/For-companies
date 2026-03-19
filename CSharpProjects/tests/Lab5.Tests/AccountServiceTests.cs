using Core.Entities;
using Core.ResultInfo;
using Core.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public class AccountServiceTests
{
    [Fact]
    public void Withdraw_WithInsufficientBalance_ReturnsFail()
    {
        var accountId = Guid.NewGuid();
        var session = UserSession.CreateUserSession(accountId);

        var sessionRepo = new MockSessionRepository();
        var accountRepo = new MockAccountRepository();

        sessionRepo.Add(session);

        var account = new Account(accountId, "1234");
        account.Deposit(new MoneyState(50m));
        accountRepo.Add(account);

        var service = new WithdrawMoneyService(accountRepo, sessionRepo);

        Result result = service.Execute(session.SessionKey, accountId, new MoneyState(100m));

        Assert.False(result.IsSuccess);
        Assert.Equal(50m, account.MoneyBalance?.Amount);
    }

    [Fact]
    public void Withdraw_WithZeroOrNegativeAmount_ReturnsFail()
    {
        var accountId = Guid.NewGuid();
        var session = UserSession.CreateUserSession(accountId);

        var sessionRepo = new MockSessionRepository();
        var accountRepo = new MockAccountRepository();

        sessionRepo.Add(session);

        var account = new Account(accountId, "1234");
        account.Deposit(new MoneyState(50m));
        accountRepo.Add(account);

        var service = new WithdrawMoneyService(accountRepo, sessionRepo);

        Result resultZero = service.Execute(session.SessionKey, accountId, new MoneyState(0m));
        Result resultNegative = service.Execute(session.SessionKey, accountId, new MoneyState(-10m));

        Assert.False(resultZero.IsSuccess);
        Assert.False(resultNegative.IsSuccess);
    }

    [Fact]
    public void Deposit_WithZeroOrNegativeAmount_ReturnsFail()
    {
        var accountId = Guid.NewGuid();
        var session = UserSession.CreateUserSession(accountId);

        var sessionRepo = new MockSessionRepository();
        var accountRepo = new MockAccountRepository();

        sessionRepo.Add(session);

        var account = new Account(accountId, "1234");
        accountRepo.Add(account);

        var service = new DepositMoneyService(sessionRepo, accountRepo);

        Result resultZero = service.Execute(session.SessionKey, accountId, new MoneyState(0m));
        Result resultNegative = service.Execute(session.SessionKey, accountId, new MoneyState(-10m));

        Assert.False(resultZero.IsSuccess);
        Assert.False(resultNegative.IsSuccess);
    }

    [Fact]
    public void Operation_Fails_WhenSessionIsInvalid()
    {
        var accountId = Guid.NewGuid();
        var accountRepo = new MockAccountRepository();
        var account = new Account(accountId, "1234");
        accountRepo.Add(account);

        var sessionRepo = new MockSessionRepository();
        var service = new WithdrawMoneyService(accountRepo, sessionRepo);

        var fakeSessionKey = Guid.NewGuid();
        Result result = service.Execute(fakeSessionKey, accountId, new MoneyState(10m));

        Assert.False(result.IsSuccess);
        Assert.Equal(0m, account.MoneyBalance?.Amount);
    }

    [Fact]
    public void Operation_Fails_WhenSessionHasNoAccess()
    {
        var accountId = Guid.NewGuid();
        var anotherAccountId = Guid.NewGuid();
        var session = UserSession.CreateUserSession(anotherAccountId);

        var sessionRepo = new MockSessionRepository();
        sessionRepo.Add(session);

        var accountRepo = new MockAccountRepository();
        var account = new Account(accountId, "1234");
        accountRepo.Add(account);

        var service = new WithdrawMoneyService(accountRepo, sessionRepo);

        Result result = service.Execute(session.SessionKey, accountId, new MoneyState(10m));

        Assert.False(result.IsSuccess);
        Assert.Equal(0m, account.MoneyBalance?.Amount);
    }

    [Fact]
    public void UserSession_Creation_WorksAndFailsOnWrongPin()
    {
        var accountId = Guid.NewGuid();
        var accountRepo = new MockAccountRepository();
        var account = new Account(accountId, "1234");
        accountRepo.Add(account);

        var sessionRepo = new MockSessionRepository();
        var service = new CreateUserSession(sessionRepo, accountRepo);

        ResultType<UserSession> successResult = service.Execute(accountId, "1234");
        Assert.True(successResult.IsSuccess);
        Assert.NotNull(successResult.Value);

        ResultType<UserSession> failResult = service.Execute(accountId, "wrong-pin");
        Assert.False(failResult.IsSuccess);
    }

    [Fact]
    public void AdminSession_Creation_WorksAndFailsOnWrongPassword()
    {
        var sessionRepo = new MockSessionRepository();
        IOptions<AdminOptions> options = Options.Create(new AdminOptions { SystemPassword = "supersecret" });
        var service = new CreateAdminSession(sessionRepo, options);

        ResultType<UserSession> successResult = service.Execute("supersecret");
        Assert.True(successResult.IsSuccess);

        ResultType<UserSession> failResult = service.Execute("wrongpassword");
        Assert.False(failResult.IsSuccess);
    }

    [Fact]
    public void CreateAccount_RequiresAdminSession()
    {
        var accountRepo = new MockAccountRepository();
        var sessionRepo = new MockSessionRepository();
        var service = new CreateAccountService(accountRepo, sessionRepo);

        var userSession = UserSession.CreateUserSession(Guid.NewGuid());
        sessionRepo.Add(userSession);

        ResultType<Account> failResult = service.Execute(userSession.SessionKey, "1234");
        Assert.False(failResult.IsSuccess);

        var adminSession = UserSession.CreateAdminSession();
        sessionRepo.Add(adminSession);
        ResultType<Account> successResult = service.Execute(adminSession.SessionKey, "1234");
        Assert.True(successResult.IsSuccess);
    }
}
