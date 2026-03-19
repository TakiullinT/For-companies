using Itmo.ObjectOrientedProgramming.Lab3.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;

public class TestCreature : Creature
{
    public int DamageTaken { get; private set; } = 0;

    public int AttacksPerformed { get; private set; } = 0;

    public TestCreature(string name, int attack, int health) : base(name, attack, health) { }

    public override void AttackCreature(ICreature creature)
    {
        AttacksPerformed++;
        creature.TakeDamage(Attack);
    }

    public override void TakeDamage(int damage)
    {
        DamageTaken += damage;
        Health = Math.Max(0, Health - damage);
    }

    public override ICreature Clone()
    {
        var clone = new TestCreature(Name, Attack, Health);
        clone.DamageTaken = DamageTaken;
        clone.AttacksPerformed = AttacksPerformed;
        return clone;
    }
}
