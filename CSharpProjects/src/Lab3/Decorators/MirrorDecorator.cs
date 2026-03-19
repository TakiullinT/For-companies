using Itmo.ObjectOrientedProgramming.Lab3.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Decorators;

public class MirrorDecorator : ICreature
{
    public ICreature BaseCreature { get; private set; }

    public MirrorDecorator(ICreature baseCreature)
    {
        BaseCreature = baseCreature;
    }

    public string Name => BaseCreature.Name;

    public bool IsAlive => BaseCreature.IsAlive;

    public int Attack => BaseCreature.Health;

    public int Health => BaseCreature.Attack;

    public void AttackCreature(ICreature creature)
    {
        creature.TakeDamage(Attack);
    }

    public void TakeDamage(int damage)
    {
        BaseCreature.TakeDamage(damage);
    }

    public ICreature Clone()
    {
        return new MirrorDecorator(BaseCreature.Clone());
    }
}