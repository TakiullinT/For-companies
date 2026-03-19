using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;
using Itmo.ObjectOrientedProgramming.Lab3.Modifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Factories;

public static class AmuletMasterFactory
{
    public static ICreature CreateCreature()
    {
        ICreature baseCreature = new AmuletMaster();

        baseCreature = new MagicShieldModifier(baseCreature);
        baseCreature = new DoubleAttackModifier(baseCreature);

        return baseCreature;
    }
}
