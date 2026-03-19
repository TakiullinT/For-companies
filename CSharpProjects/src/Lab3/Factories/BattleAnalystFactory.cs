using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;

namespace Itmo.ObjectOrientedProgramming.Lab3.Factories;

public static class BattleAnalystFactory
{
    public static ICreature CreateCreature()
    {
        return new BattleAnalyst();
    }
}