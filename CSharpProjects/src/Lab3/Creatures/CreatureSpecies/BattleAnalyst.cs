namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;

public class BattleAnalyst : Creature
{
    private bool _hasBoosted = false;

    public BattleAnalyst() : base("Боевой аналитик", 2, 4) { }

    public override void AttackCreature(ICreature creature)
    {
        if (!IsAlive || Attack < 0) return;
        if (!_hasBoosted)
        {
            Attack += 2;
            _hasBoosted = true;
        }

        creature.TakeDamage(Attack);
    }

    public override ICreature Clone()
    {
        var clone = new BattleAnalyst();
        clone.Attack = Attack;
        clone.Health = Health;
        if (_hasBoosted) clone._hasBoosted = true;
        return clone;
    }
}
