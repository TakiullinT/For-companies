using Itmo.ObjectOrientedProgramming.Lab3.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Modifiers;

public class DoubleAttackModifier : ICreature
{
    public ICreature BaseCreature { get; private set; }

    public DoubleAttackModifier(ICreature baseCreature)
    {
        BaseCreature = baseCreature;
    }

    public string Name => BaseCreature.Name;

    public int Attack => BaseCreature.Attack;

    public int Health => BaseCreature.Health;

    public bool IsAlive => BaseCreature.IsAlive;

    public void AttackCreature(ICreature creature)
    {
        if (!BaseCreature.IsAlive || BaseCreature.Attack <= 0) return;

        creature.TakeDamage(BaseCreature.Attack);

        if (BaseCreature.IsAlive)
        {
            creature.TakeDamage(BaseCreature.Attack);
        }
    }

    public void TakeDamage(int damage)
    {
        BaseCreature.TakeDamage(damage);
    }

    public ICreature Clone()
    {
        return new DoubleAttackModifier(BaseCreature.Clone());
    }
}