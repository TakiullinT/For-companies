using Itmo.ObjectOrientedProgramming.Lab3.Decorators;
using Itmo.ObjectOrientedProgramming.Lab3.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class DecoratorsTests
{
    [Fact]
    public void StatModifierDecorator_AdjustsStatsAndClones_AttackIsAdjusted()
    {
        var baseCreature = new TestCreature("Base", 2, 3);
        var statDecorator = new StatModifierDecorator(baseCreature, attackDelta: 3, healthDelta: 4);

        Assert.Equal(5, statDecorator.Attack);
    }

    [Fact]
    public void StatModifierDecorator_AdjustsStatsAndClones_HealthIsAdjusted()
    {
        var baseCreature = new TestCreature("Base", 2, 3);
        var statDecorator = new StatModifierDecorator(baseCreature, attackDelta: 3, healthDelta: 4);

        Assert.Equal(5, statDecorator.Attack);
        Assert.Equal(7, statDecorator.Health);
    }

    [Fact]
    public void StatModifierDecorator_AdjustsStatsAndClones_HealthIsIndependentOnClone()
    {
        var baseCreature = new TestCreature("Base", 2, 3);
        var statDecorator = new StatModifierDecorator(baseCreature, attackDelta: 3, healthDelta: 4);
        var clone = (StatModifierDecorator)statDecorator.Clone();
        clone.TakeDamage(2);

        Assert.Equal(5, statDecorator.Attack);
        Assert.Equal(7, statDecorator.Health);
        Assert.NotEqual(statDecorator.Health, clone.Health);
    }

    [Fact]
    public void StatModifierDecorator_AdjustsStatsAndClones_ReferencesAreNotEqual()
    {
        var baseCreature = new TestCreature("Base", 2, 3);
        var statDecorator = new StatModifierDecorator(baseCreature, attackDelta: 3, healthDelta: 4);
        var clone = (StatModifierDecorator)statDecorator.Clone();
        clone.TakeDamage(2);

        Assert.Equal(7, statDecorator.Health);
        Assert.NotEqual(statDecorator.Health, clone.Health);
        Assert.False(ReferenceEquals(statDecorator, clone));
    }

    [Fact]
    public void StatModifierDecorator_AdjustsAttackAndHealth_AndIsIndependentOnClone()
    {
        var baseCreature = new TestCreature("Base", 2, 3);
        var statDecorator = new StatModifierDecorator(baseCreature, attackDelta: 3, healthDelta: 4);

        Assert.Equal(5, statDecorator.Attack);
        Assert.Equal(7, statDecorator.Health);

        var clone = (StatModifierDecorator)statDecorator.Clone();
        clone.TakeDamage(2);

        Assert.Equal(7, statDecorator.Health);
        Assert.NotEqual(statDecorator.Health, clone.Health);
        Assert.False(ReferenceEquals(statDecorator, clone));
    }

    [Fact]
    public void StatModifierDecorator_AttackCreature_UsesThisAttack_NotBaseAttack()
    {
        var baseCreature = new TestCreature("B", 2, 4);
        var statDecorator = new StatModifierDecorator(baseCreature, attackDelta: 5, healthDelta: 0);

        var enemy = new TestCreature("Enemy", 1, 20);

        statDecorator.AttackCreature(enemy);

        Assert.Equal(13, enemy.Health);
    }

    [Fact]
    public void MirrorDecorator_SwapsAttackAndHealth_AndCloneIsIndependent()
    {
        var baseCreature = new TestCreature("Swap", 2, 6);
        var mirror = new MirrorDecorator(baseCreature);

        Assert.Equal(6, mirror.Attack);
        Assert.Equal(2, mirror.Health);

        var clone = (MirrorDecorator)mirror.Clone();
        clone.TakeDamage(1);

        Assert.Equal(2, mirror.Health);
        Assert.Equal(mirror.Health, clone.Health);
    }

    [Fact]
    public void CreatureDecorator_TakeDamage_DelegatesToBaseUnlessOverridden()
    {
        var baseCreature = new TestCreature("C", 3, 8);
        var stat = new StatModifierDecorator(baseCreature, 2, 3);

        stat.TakeDamage(4);

        Assert.Equal(7, stat.Health);
    }

    [Fact]
    public void MagicShieldModifier_Clone_PreservesFlag()
    {
        var baseCreature = new TestCreature("S", 1, 5);
        var shield = new MagicShieldModifier(baseCreature);

        shield.TakeDamage(1);
        Assert.Equal(5, shield.Health);

        var clone = (MagicShieldModifier)shield.Clone();

        clone.TakeDamage(1);
        Assert.Equal(4, clone.Health);

        Assert.Equal(5, shield.Health);
    }
}