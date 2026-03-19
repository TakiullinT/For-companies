using Itmo.ObjectOrientedProgramming.Lab3.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Modifiers;

public class MagicShieldModifier : ICreature
{
    public ICreature BaseCreature { get; private set; }

    private bool _isShieldUsed = false;

    public string Name => BaseCreature.Name;

    public int Attack => BaseCreature.Attack;

    public int Health => BaseCreature.Health;

    public bool IsAlive => BaseCreature.IsAlive;

    public MagicShieldModifier(ICreature baseCreature)
    {
        BaseCreature = baseCreature;
    }

    public void AttackCreature(ICreature creature)
    {
        creature.TakeDamage(BaseCreature.Attack);
    }

    public void TakeDamage(int damage)
    {
        if (!_isShieldUsed)
        {
            _isShieldUsed = true;
            return;
        }

        BaseCreature.TakeDamage(damage);
    }

    public ICreature Clone()
    {
        var clone = new MagicShieldModifier(BaseCreature.Clone());
        if (_isShieldUsed)
        {
            clone._isShieldUsed = true;
        }

        return clone;
    }
}