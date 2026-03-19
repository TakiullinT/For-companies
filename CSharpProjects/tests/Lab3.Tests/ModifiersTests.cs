using Itmo.ObjectOrientedProgramming.Lab3.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class ModifiersTests
{
    [Fact]
    public void MagicShield_IgnoresFirstHit_HealthIsUnchanged()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);
        shielded.TakeDamage(3);

        Assert.Equal(5, shielded.Health);
    }

    [Fact]
    public void MagicShield_IgnoresFirstHit_IsAlive()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);

        shielded.TakeDamage(3);

        Assert.Equal(5, shielded.Health);
        Assert.True(shielded.IsAlive);
    }

    [Fact]
    public void MagicShield_AllowsSecondHit_HealthIsReducedBySecondHit()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);
        shielded.TakeDamage(1);
        shielded.TakeDamage(2);

        Assert.Equal(3, shielded.Health);
    }

    [Fact]
    public void MagicShield_AllowsSecondHit_IsAliveAfterSecondHit()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);

        shielded.TakeDamage(1);
        shielded.TakeDamage(2);

        Assert.Equal(3, shielded.Health);
        Assert.True(shielded.IsAlive);
    }

    [Fact]
    public void MagicShield_Stacking_FirstHitIsIgnored()
    {
        var baseCreature = new TestCreature("MultiShield", 2, 5);
        var doubleShield = new MagicShieldModifier(new MagicShieldModifier(baseCreature));

        doubleShield.TakeDamage(3);
        Assert.Equal(5, doubleShield.Health);
    }

    [Fact]
    public void MagicShield_Stacking_SecondHitIsIgnored()
    {
        var baseCreature = new TestCreature("MultiShield", 2, 5);
        var doubleShield = new MagicShieldModifier(new MagicShieldModifier(baseCreature));

        doubleShield.TakeDamage(3);
        Assert.Equal(5, doubleShield.Health);

        doubleShield.TakeDamage(3);
        Assert.Equal(5, doubleShield.Health);
    }

    [Fact]
    public void MagicShield_Stacking_DamageAppliedAfterTwoShields()
    {
        var baseCreature = new TestCreature("MultiShield", 2, 5);
        var doubleShield = new MagicShieldModifier(new MagicShieldModifier(baseCreature));

        doubleShield.TakeDamage(3);
        Assert.Equal(5, doubleShield.Health);

        doubleShield.TakeDamage(3);
        Assert.Equal(5, doubleShield.Health);

        doubleShield.TakeDamage(2);
        Assert.Equal(3, doubleShield.Health);
    }

    [Fact]
    public void MagicShield_ClonePreservesFlag_OriginalShieldIsUsed()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);

        shielded.TakeDamage(1);
        Assert.Equal(5, shielded.Health);
    }

    [Fact]
    public void MagicShield_ClonePreservesFlag_CloneTakesDamage()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);
        shielded.TakeDamage(1);

        var clone = (MagicShieldModifier)shielded.Clone();
        clone.TakeDamage(1);

        Assert.Equal(4, clone.Health);
    }

    [Fact]
    public void MagicShield_ClonePreservesFlag_OriginalHealthIsUnchangedByClone()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);
        shielded.TakeDamage(1);

        var clone = (MagicShieldModifier)shielded.Clone();
        clone.TakeDamage(1);

        Assert.Equal(4, clone.Health);
        Assert.Equal(5, shielded.Health);
    }

    [Fact]
    public void MagicShield_ClonePreservesFlag_ReferencesAreIndependent()
    {
        var baseCreature = new TestCreature("Shielded", 2, 5);
        var shielded = new MagicShieldModifier(baseCreature);

        shielded.TakeDamage(1);

        var clone = (MagicShieldModifier)shielded.Clone();

        clone.TakeDamage(1);
        Assert.Equal(4, clone.Health);

        Assert.Equal(5, shielded.Health);
        Assert.NotSame(shielded, clone);
    }

    [Fact]
    public void DoubleAttack_PerformsTwoAttacks_DefenderHealthReducedByDoubleAttack()
    {
        var attacker = new TestCreature("Attacker", 3, 5);
        var doubleAttack = new DoubleAttackModifier(attacker);

        var defender = new TestCreature("Defender", 1, 10);

        doubleAttack.AttackCreature(defender);

        Assert.Equal(4, defender.Health);
    }

    [Fact]
    public void DoubleAttack_WithZeroAttack_DefenderHealthIsUnchanged()
    {
        var attacker = new TestCreature("Weak", 0, 5);
        var doubleAttack = new DoubleAttackModifier(attacker);

        var defender = new TestCreature("Target", 1, 6);

        doubleAttack.AttackCreature(defender);

        Assert.Equal(6, defender.Health);
    }

    [Fact]
    public void DoubleAttack_CloneIsIndependent_CloneAttackReducesTarget1Health()
    {
        var attacker = new TestCreature("Attacker", 3, 5);
        var doubleAttack = new DoubleAttackModifier(attacker);

        var clone = (DoubleAttackModifier)doubleAttack.Clone();
        var target1 = new TestCreature("T1", 1, 10);

        clone.AttackCreature(target1);

        Assert.Equal(4, target1.Health);
    }

    [Fact]
    public void DoubleAttack_CloneIsIndependent_OriginalAttackReducesTarget2Health()
    {
        var attacker = new TestCreature("Attacker", 3, 5);
        var doubleAttack = new DoubleAttackModifier(attacker);

        var clone = (DoubleAttackModifier)doubleAttack.Clone();
        var target1 = new TestCreature("T1", 1, 10);
        var target2 = new TestCreature("T2", 1, 10);

        clone.AttackCreature(target1);
        Assert.Equal(4, target1.Health);

        doubleAttack.AttackCreature(target2);
        Assert.Equal(4, target2.Health);
    }

    [Fact]
    public void DoubleAttack_CloneIsIndependent_ReferencesAreIndependent()
    {
        var attacker = new TestCreature("Attacker", 3, 5);
        var doubleAttack = new DoubleAttackModifier(attacker);

        var clone = (DoubleAttackModifier)doubleAttack.Clone();
        var target1 = new TestCreature("T1", 1, 10);
        var target2 = new TestCreature("T2", 1, 10);

        clone.AttackCreature(target1);
        Assert.Equal(4, target1.Health);

        doubleAttack.AttackCreature(target2);
        Assert.Equal(4, target2.Health);

        Assert.NotSame(doubleAttack, clone);
    }

    [Fact]
    public void Decorator_TakeDamageDelegates_FirstHitIsInterceptedByShield()
    {
        var baseCreature = new TestCreature("C", 2, 6);
        var chain = new DoubleAttackModifier(new MagicShieldModifier(baseCreature));

        chain.TakeDamage(2);
        Assert.Equal(6, chain.Health);
    }

    [Fact]
    public void Decorator_TakeDamageDelegates_SecondHitIsAppliedToBase()
    {
        var baseCreature = new TestCreature("C", 2, 6);
        var chain = new DoubleAttackModifier(new MagicShieldModifier(baseCreature));

        chain.TakeDamage(2);
        Assert.Equal(6, chain.Health);

        chain.TakeDamage(3);
        Assert.Equal(3, chain.Health);
    }
}