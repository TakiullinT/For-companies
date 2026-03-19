using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab3.PlayerTableInfo;

public interface IPlayerTable
{
    IReadOnlyList<ICreature> Creatures { get; }

    Result AddCreature(ICreature creature);

    IEnumerable<ICreature> GetAttackers();

    IEnumerable<ICreature> GetDefenders();
}