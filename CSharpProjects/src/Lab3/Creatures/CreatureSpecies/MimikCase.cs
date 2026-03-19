namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;

public class MimikCase : Creature
{
    public MimikCase() : base("Сундук-мимик", 1, 1) { }

    private MimikCase(int attack, int health) : base("Сундук-мимик", attack, health) { }

    public override void AttackCreature(ICreature creature)
    {
        int newAttack = Math.Max(Attack, creature.Attack);
        int newHealth = Math.Max(Health, creature.Health);

        Attack = newAttack;
        Health = newHealth;

        creature.TakeDamage(Attack);
    }

    public override ICreature Clone()
    {
        return new MimikCase(Attack, Health);
    }
}