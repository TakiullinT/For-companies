using Core.Entities;

namespace Core.Ports;

public interface IAccountRepository
{
    Account? GetById(Guid id);

    void Save(Account account);
}