namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures;

public interface ICreature
{
    string Name { get; }

    int Attack { get; }

    int Health { get; }

    bool IsAlive { get; }

    void AttackCreature(ICreature creature);

    void TakeDamage(int damage);

    ICreature Clone();
}