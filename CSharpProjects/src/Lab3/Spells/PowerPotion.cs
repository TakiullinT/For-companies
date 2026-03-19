using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Decorators;

namespace Itmo.ObjectOrientedProgramming.Lab3.Spells;

public class PowerPotion : ISpell
{
    public ICreature Apply(ICreature creature)
    {
        return new StatModifierDecorator(creature.Clone(), 5, 0);
    }
}