using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab3.PlayerTableInfo;

public class PlayerTable : IPlayerTable
{
    private const int MaxCreaturesAmount = 7;

    private readonly List<ICreature> _creatures = new();

    public IReadOnlyList<ICreature> Creatures => _creatures;

    public Result AddCreature(ICreature? creature)
    {
        if (creature is null)
        {
            return Result.Fail("Существо не может быть null");
        }

        if (_creatures.Count >= MaxCreaturesAmount)
        {
            return Result.Fail("На столе не может быть более 7 существ");
        }

        _creatures.Add(creature.Clone());

        return Result.Success();
    }

    public IEnumerable<ICreature> GetAttackers()
    {
        return _creatures.Where(creature => creature.IsAlive && creature.Attack > 0);
    }

    public IEnumerable<ICreature> GetDefenders()
    {
        return _creatures.Where(creature => creature.IsAlive);
    }
}
