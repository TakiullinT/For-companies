using Core.Entities;
using Core.Ports;
using System.Collections.Concurrent;

namespace Lab5.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    public ConcurrentDictionary<Guid, Account> Accounts { get; private set; } = new();

    public Account? GetById(Guid id)
    {
        Accounts.TryGetValue(id, out Account? account);
        return account;
    }

    public void Save(Account account)
    {
        Accounts[account.Id] = account;
    }
}