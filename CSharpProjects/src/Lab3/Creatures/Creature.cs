namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures;

public abstract class Creature : ICreature
{
    public virtual string Name { get; protected set; }

    public virtual int Attack { get; protected set; }

    public virtual int Health { get; protected set; }

    public virtual bool IsAlive => Health > 0;

    protected Creature(string name, int attack, int health)
    {
        Name = name;
        Attack = attack;
        Health = health;
    }

    public virtual void AttackCreature(ICreature creature)
    {
        if (IsAlive && Attack > 0) creature.TakeDamage(Attack);
    }

    public virtual void TakeDamage(int damage)
    {
        Health = Math.Max(0, Health - damage);
    }

    public abstract ICreature Clone();
}