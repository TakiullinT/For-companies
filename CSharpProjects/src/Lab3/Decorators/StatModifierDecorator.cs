using Itmo.ObjectOrientedProgramming.Lab3.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Decorators;

public class StatModifierDecorator : ICreature
{
    public ICreature BaseCreature { get; private set; }

    public int CurrentHealth { get; private set; }

    public int AttackDelta { get; private set; }

    public int HealthDelta { get; private set; }

    public string Name => BaseCreature.Name;

    public int Attack => BaseCreature.Attack + AttackDelta;

    public int Health => BaseCreature.Health + HealthDelta;

    public bool IsAlive => CurrentHealth > 0;

    public StatModifierDecorator(ICreature baseCreature, int attackDelta, int healthDelta)
    {
        AttackDelta = attackDelta;
        HealthDelta = healthDelta;
        BaseCreature = baseCreature;
        CurrentHealth = baseCreature.Health + healthDelta;
    }

    public void TakeDamage(int damage)
    {
        BaseCreature.TakeDamage(damage);
    }

    public void AttackCreature(ICreature creature)
    {
        creature.TakeDamage(Attack);
    }

    public ICreature Clone()
    {
        return new StatModifierDecorator(BaseCreature.Clone(), AttackDelta, HealthDelta);
    }
}