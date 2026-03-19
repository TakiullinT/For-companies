using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Decorators;
using Itmo.ObjectOrientedProgramming.Lab3.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class CreatureCloneTests
{
    [Fact]
    public void Creature_CloneIndependent_FirstCheck_CreatureHealthUnchanged()
    {
        var creature = new TestCreature("A", 3, 5);
        ICreature clone = creature.Clone();

        clone.TakeDamage(3);

        Assert.Equal(5, creature.Health);
        Assert.Equal(2, clone.Health);
    }

    [Fact]
    public void Creature_CloneIndependent_SecondCheck_CloneHealthReduced()
    {
        var creature = new TestCreature("A", 3, 5);
        ICreature clone = creature.Clone();

        clone.TakeDamage(3);

        Assert.Equal(5, creature.Health);
        Assert.Equal(2, clone.Health);
    }

    [Fact]
    public void Creature_Clone_ShouldBeIndependent_OriginalHealthUnchanged()
    {
        var original = new TestCreature("A", 3, 5);
        var clone = (Creature)original.Clone();

        clone.TakeDamage(2);

        Assert.Equal(5, original.Health);
    }

    [Fact]
    public void Creature_Clone_ShouldBeIndependent_AttackSame()
    {
        var original = new TestCreature("A", 3, 5);
        var clone = (Creature)original.Clone();

        clone.TakeDamage(2);

        Assert.Equal(5, original.Health);
        Assert.Equal(3, clone.Attack);
    }

    [Fact]
    public void Creature_Clone_ShouldBeIndependent_CloneHealthReduced()
    {
        var original = new TestCreature("A", 3, 5);
        var clone = (Creature)original.Clone();

        clone.TakeDamage(2);

        Assert.Equal(5, original.Health);
        Assert.Equal(3, clone.Attack);
        Assert.Equal(3, clone.Health);
    }

    [Fact]
    public void Creature_Clone_ShouldBeIndependent_NotSameReference()
    {
        var original = new TestCreature("A", 3, 5);
        var clone = (Creature)original.Clone();

        clone.TakeDamage(2);

        Assert.Equal(5, original.Health);
        Assert.Equal(3, original.Attack);
        Assert.Equal(3, clone.Health);
        Assert.NotSame(original, clone);
    }

    [Fact]
    public void Creature_TakeDamage_FirstHit_ReducesHealth()
    {
        var creature = new TestCreature("C", 2, 5);
        creature.TakeDamage(2);
        Assert.Equal(3, creature.Health);
    }

    [Fact]
    public void Creature_TakeDamage_FirstHit_StillAlive()
    {
        var creature = new TestCreature("C", 2, 5);
        creature.TakeDamage(2);
        Assert.Equal(3, creature.Health);
        Assert.True(creature.IsAlive);
    }

    [Fact]
    public void Creature_TakeDamage_SecondHit_CannotGoBelowZero()
    {
        var creature = new TestCreature("C", 2, 5);

        creature.TakeDamage(2);
        Assert.Equal(3, creature.Health);
        Assert.True(creature.IsAlive);

        creature.TakeDamage(10);
        Assert.Equal(0, creature.Health);
    }

    [Fact]
    public void Creature_TakeDamage_SecondHit_DeadAfterOverkill()
    {
        var creature = new TestCreature("C", 2, 5);

        creature.TakeDamage(2);
        Assert.Equal(3, creature.Health);
        Assert.True(creature.IsAlive);

        creature.TakeDamage(10);
        Assert.Equal(0, creature.Health);
        Assert.False(creature.IsAlive);
    }

    [Fact]
    public void Creature_AttackWithZeroAttack_TargetHealthUnchanged()
    {
        var attacker = new TestCreature("Weak", 0, 5);
        var defender = new TestCreature("Target", 1, 6);

        attacker.AttackCreature(defender);

        Assert.Equal(6, defender.Health);
    }

    [Fact]
    public void Creature_ModifierChain_DoubleAttack_AppliesCorrectDamage()
    {
        ICreature baseCreature = new TestCreature("Base", 3, 5);
        var modified = new DoubleAttackModifier(new MagicShieldModifier(baseCreature));

        ICreature clone = modified.Clone();
        var target = new TestCreature("Target", 2, 6);

        modified.AttackCreature(target);
        Assert.Equal(0, target.Health);
    }

    [Fact]
    public void Creature_ModifierChain_FirstShieldBlocksDamage()
    {
        ICreature baseCreature = new TestCreature("Base", 3, 5);
        var modified = new DoubleAttackModifier(new MagicShieldModifier(baseCreature));

        ICreature clone = modified.Clone();
        var target = new TestCreature("Target", 2, 6);

        modified.AttackCreature(target);
        Assert.Equal(0, target.Health);

        modified.TakeDamage(2);
        Assert.Equal(5, modified.Health);
    }

    [Fact]
    public void Creature_ModifierChain_SecondHitDamagesAfterShieldUsed()
    {
        ICreature baseCreature = new TestCreature("Base", 3, 5);
        var modified = new DoubleAttackModifier(new MagicShieldModifier(baseCreature));

        ICreature clone = modified.Clone();
        var target = new TestCreature("Target", 2, 6);

        modified.AttackCreature(target);
        Assert.Equal(0, target.Health);

        modified.TakeDamage(2);
        Assert.Equal(5, modified.Health);

        modified.TakeDamage(2);
        Assert.Equal(3, modified.Health);
    }

    [Fact]
    public void Creature_ModifierChain_CloneIndependent()
    {
        ICreature baseCreature = new TestCreature("Base", 3, 5);
        var modified = new DoubleAttackModifier(new MagicShieldModifier(baseCreature));

        ICreature clone = modified.Clone();
        var target = new TestCreature("Target", 2, 6);

        modified.AttackCreature(target);
        Assert.Equal(0, target.Health);

        modified.TakeDamage(2);
        Assert.Equal(5, modified.Health);

        modified.TakeDamage(2);
        Assert.Equal(3, modified.Health);

        clone.TakeDamage(2);
        Assert.Equal(5, clone.Health);
    }

    [Fact]
    public void Creature_AttackCreature_DefenderHealthReduced()
    {
        var attacker = new TestCreature("Attacker", 2, 5);
        var defender = new TestCreature("Defender", 1, 6);

        attacker.AttackCreature(defender);

        Assert.Equal(4, defender.Health);
    }

    [Fact]
    public void Creature_AttackCreature_AttackerHealthUnchanged()
    {
        var attacker = new TestCreature("Attacker", 2, 5);
        var defender = new TestCreature("Defender", 1, 6);

        attacker.AttackCreature(defender);

        Assert.Equal(4, defender.Health);
        Assert.Equal(5, attacker.Health);
    }

    [Fact]
    public void Creature_AttackCreature_AttackerStillAlive()
    {
        var attacker = new TestCreature("Attacker", 2, 5);
        var defender = new TestCreature("Defender", 1, 6);

        attacker.AttackCreature(defender);

        Assert.Equal(4, defender.Health);
        Assert.Equal(5, attacker.Health);
        Assert.True(attacker.IsAlive);
    }

    [Fact]
    public void CreatureBuilder_ModifiersApplied_CorrectAttack()
    {
        CreatureBuilder builder = new CreatureBuilder((name, attack, health) => new TestCreature(name, attack, health), "B")
            .WithStats(1, 1)
            .WithModifier(creature => new StatModifierDecorator(creature, +2, 0))
            .WithModifier(creature => new StatModifierDecorator(creature, 0, +3));

        ICreature creature = builder.Build();
        Assert.Equal(3, creature.Attack);
    }

    [Fact]
    public void CreatureBuilder_ModifiersApplied_CorrectHealth()
    {
        CreatureBuilder builder = new CreatureBuilder((name, attack, health) => new TestCreature(name, attack, health), "B")
            .WithStats(1, 1)
            .WithModifier(creature => new StatModifierDecorator(creature, +2, 0))
            .WithModifier(creature => new StatModifierDecorator(creature, 0, +3));

        ICreature creature = builder.Build();
        Assert.Equal(3, creature.Attack);
        Assert.Equal(4, creature.Health);
    }

    [Fact]
    public void CreaturesCatalog_Create_ExistingKey_Success()
    {
        var catalog = new CreaturesCatalog();
        var template = new MagicShieldModifier(new TestCreature("T", 2, 3));
        catalog.Register("shielded", template);

        ResultType<ICreature> result1 = catalog.Create("shielded");
        Assert.True(result1.IsSuccess);
    }

    [Fact]
    public void CreaturesCatalog_Create_CloneIndependent()
    {
        var catalog = new CreaturesCatalog();
        var template = new MagicShieldModifier(new TestCreature("T", 2, 3));
        catalog.Register("shielded", template);

        ResultType<ICreature> result1 = catalog.Create("shielded");
        Assert.True(result1.IsSuccess);
        ICreature? clone1 = result1.Value;
        clone1?.TakeDamage(1);

        ResultType<ICreature> result2 = catalog.Create("shielded");
        ICreature? clone2 = result2.Value;
        clone2?.TakeDamage(1);
        if (clone2 != null) Assert.Equal(3, clone2.Health);
    }

    [Fact]
    public void CreaturesCatalog_Create_MissingKey_Failure()
    {
        var catalog = new CreaturesCatalog();
        var template = new MagicShieldModifier(new TestCreature("T", 2, 3));
        catalog.Register("shielded", template);

        ResultType<ICreature> result1 = catalog.Create("shielded");
        Assert.True(result1.IsSuccess);
        ICreature? clone1 = result1.Value;
        clone1?.TakeDamage(1);

        ResultType<ICreature> result2 = catalog.Create("shielded");
        ICreature? clone2 = result2.Value;
        clone2?.TakeDamage(1);
        if (clone2 != null) Assert.Equal(3, clone2.Health);

        ResultType<ICreature> result3 = catalog.Create("absent");
        Assert.False(result3.IsSuccess);
    }
}