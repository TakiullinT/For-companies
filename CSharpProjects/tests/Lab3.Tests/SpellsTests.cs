using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Spells;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class SpellsTests
{
    [Fact]
    public void HealingPotion_IncreasesHealth_HealedHealthIsEight()
    {
        var creature = new TestCreature("A", 2, 3);
        var potion = new HealingPotion();

        ICreature healed = potion.Apply(creature);

        Assert.Equal(8, healed.Health);
    }

    [Fact]
    public void HealingPotion_IncreasesHealth_OriginalCreatureIsUnchanged()
    {
        var creature = new TestCreature("A", 2, 3);
        var potion = new HealingPotion();

        ICreature healed = potion.Apply(creature);

        Assert.Equal(8, healed.Health);
        Assert.Equal(3, creature.Health);
    }

    [Fact]
    public void MagicMirror_SwapsStats_MirroredAttackEqualsOriginalHealth()
    {
        var creature = new TestCreature("A", 2, 6);
        var mirror = new MagicMirror();

        ICreature mirrored = mirror.Apply(creature);

        Assert.Equal(6, mirrored.Attack);
    }

    [Fact]
    public void MagicMirror_SwapsStats_MirroredHealthEqualsOriginalAttack()
    {
        var creature = new TestCreature("A", 2, 6);
        var mirror = new MagicMirror();

        ICreature mirrored = mirror.Apply(creature);

        Assert.Equal(6, mirrored.Attack);
        Assert.Equal(2, mirrored.Health);
    }

    [Fact]
    public void MagicMirror_SwapsStats_OriginalAttackIsUnchanged()
    {
        var creature = new TestCreature("A", 2, 6);
        var mirror = new MagicMirror();

        ICreature mirrored = mirror.Apply(creature);

        Assert.Equal(6, mirrored.Attack);
        Assert.Equal(2, mirrored.Health);
        Assert.Equal(2, creature.Attack);
    }

    [Fact]
    public void MagicMirror_SwapsStats_OriginalHealthIsUnchanged()
    {
        var creature = new TestCreature("A", 2, 6);
        var mirror = new MagicMirror();

        ICreature mirrored = mirror.Apply(creature);

        Assert.Equal(6, mirrored.Attack);
        Assert.Equal(2, mirrored.Health);
        Assert.Equal(2, creature.Attack);
        Assert.Equal(6, creature.Health);
    }

    [Fact]
    public void PowerPotion_IncreasesAttack_BuffedAttackIsSeven()
    {
        var creature = new TestCreature("A", 2, 6);
        var potion = new PowerPotion();

        ICreature buffed = potion.Apply(creature);

        Assert.Equal(7, buffed.Attack);
    }

    [Fact]
    public void PowerPotion_IncreasesAttack_OriginalAttackIsUnchanged()
    {
        var creature = new TestCreature("A", 2, 6);
        var potion = new PowerPotion();

        ICreature buffed = potion.Apply(creature);

        Assert.Equal(7, buffed.Attack);
        Assert.Equal(2, creature.Attack);
    }

    [Fact]
    public void ProtectionAmulet_ProvidesShield_HealthIsReducedOnlyBySecondHit()
    {
        var creature = new TestCreature("A", 2, 6);
        var amulet = new ProtectionAmulet();

        ICreature buffed = amulet.Apply(creature);

        buffed.TakeDamage(5);
        buffed.TakeDamage(3);

        Assert.Equal(3, buffed.Health);
    }

    [Fact]
    public void Spells_Composable_HealthIsIncreased()
    {
        var creature = new TestCreature("A", 2, 2);
        var power = new PowerPotion();
        var heal = new HealingPotion();

        ICreature buffed = power.Apply(heal.Apply(creature));

        Assert.True(buffed.Health > 2);
    }

    [Fact]
    public void Spells_Composable_AttackIsIncreased()
    {
        var creature = new TestCreature("A", 2, 2);
        var power = new PowerPotion();
        var heal = new HealingPotion();

        ICreature buffed = power.Apply(heal.Apply(creature));

        Assert.True(buffed.Health > 2);
        Assert.True(buffed.Attack > 2);
    }

    [Fact]
    public void Spells_DoNotRemoveModifiers_ShieldFunctionalityIsPreserved()
    {
        var baseC = new TestCreature("Base", 2, 5);
        var shielded = new MagicShieldModifier(baseC);
        var potion = new HealingPotion();

        ICreature buffed = potion.Apply(shielded);

        buffed.TakeDamage(3);
        Assert.Equal(10, buffed.Health);
    }
}