namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;

public class ImmortalHorror : Creature
{
    private bool _isRevived = false;

    public ImmortalHorror() : base("Бессмертный ужас", 4, 4) { }

    private ImmortalHorror(int attack, int health, bool isRevived) : base("Бессмертный ужас", attack, health)
    {
        _isRevived = isRevived;
    }

    public override void AttackCreature(ICreature creature)
    {
        creature.TakeDamage(Attack);
    }

    public override void TakeDamage(int damage)
    {
        if (!_isRevived && Health <= damage)
        {
            Health = 1;
            _isRevived = true;
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    public override ICreature Clone()
    {
        return new ImmortalHorror(Attack, Health, _isRevived);
    }
}