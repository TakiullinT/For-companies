namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;

public class EvilFighter : Creature
{
    private bool _gotNonFatalDamage = false;

    public override int Attack => base.Attack * (_gotNonFatalDamage ? 2 : 1);

    public EvilFighter() : base("Злобный боец", 1, 6) { }

    private EvilFighter(int attack, int health, bool gotNonFatalDamage) : base("Злобный боец", attack, health)
    {
        _gotNonFatalDamage = gotNonFatalDamage;
    }

    public override void AttackCreature(ICreature creature)
    {
        creature.TakeDamage(Attack);
    }

    public override void TakeDamage(int damage)
    {
        if (Health > damage && !_gotNonFatalDamage)
        {
            _gotNonFatalDamage = true;
        }

        base.TakeDamage(damage);
    }

    public override ICreature Clone()
    {
        return new EvilFighter(base.Attack, Health, _gotNonFatalDamage);
    }
}