using Core.Entities;
using Core.Ports;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public class MockAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, Account> _accounts
        = new();

    public Account? GetById(Guid id)
    {
        _accounts.TryGetValue(id, out Account? account);
        return account;
    }

    public void Save(Account account)
    {
        _accounts[account.Id] = account;
    }

    public void Add(Account account) =>
        _accounts[account.Id] = account;
}
