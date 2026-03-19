using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Decorators;

namespace Itmo.ObjectOrientedProgramming.Lab3.Spells;

public class HealingPotion : ISpell
{
    public ICreature Apply(ICreature creature)
    {
        return new StatModifierDecorator(creature.Clone(), 0, 5);
    }
}