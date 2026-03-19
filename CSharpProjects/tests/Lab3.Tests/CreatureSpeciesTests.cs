using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Factories;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class CreatureSpeciesTests
{
    [Fact]
    public void BattleAnalyst_AttackDealsBasePlusTwo_EnemyHealthReducedByCorrectAmount()
    {
        ICreature analyst = BattleAnalystFactory.CreateCreature();
        var enemy = new TestCreature("Enemy", 1, 10);

        analyst.AttackCreature(enemy);

        Assert.Equal(6, enemy.Health);
    }

    [Fact]
    public void BattleAnalyst_Clone_IsIndependent_OriginalHealthUnchanged()
    {
        ICreature analyst = BattleAnalystFactory.CreateCreature();
        ICreature clone = analyst.Clone();

        clone.TakeDamage(1);

        Assert.Equal(4, analyst.Health);
    }

    [Fact]
    public void BattleAnalyst_Clone_IsIndependent_CloneHealthReduced()
    {
        ICreature analyst = BattleAnalystFactory.CreateCreature();
        ICreature clone = analyst.Clone();

        clone.TakeDamage(1);

        Assert.Equal(4, analyst.Health);
        Assert.Equal(3, clone.Health);
    }

    [Fact]
    public void EvilFighter_NonFatalDamage_DoublesAttackValue()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(1);

        Assert.Equal(2, fighter.Attack);
    }

    [Fact]
    public void EvilFighter_NonFatalDamage_AttackAppliesCorrectDamage()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(1);

        Assert.Equal(2, fighter.Attack);

        var target = new TestCreature("Target", 1, 6);
        fighter.AttackCreature(target);

        Assert.Equal(4, target.Health);
    }

    [Fact]
    public void EvilFighter_FatalDamageKills_IsNotAlive()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(10);

        Assert.False(fighter.IsAlive);
    }

    [Fact]
    public void EvilFighter_FatalDamageKills_HealthIsZero()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(10);

        Assert.False(fighter.IsAlive);
        Assert.Equal(0, fighter.Health);
    }

    [Fact]
    public void EvilFighter_ClonePreservesFlag_OriginalAttackIsDoubled()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(1);

        Assert.Equal(2, fighter.Attack);
    }

    [Fact]
    public void EvilFighter_ClonePreservesFlag_CloneAttackMatchesOriginal()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(1);
        Assert.Equal(2, fighter.Attack);

        ICreature clone = fighter.Clone();
        Assert.Equal(fighter.Attack, clone.Attack);
    }

    [Fact]
    public void EvilFighter_ClonePreservesFlag_CloneStateIsIndependent()
    {
        ICreature fighter = EvilFighterFactory.CreateCreature();
        fighter.TakeDamage(1);
        Assert.Equal(2, fighter.Attack);

        ICreature clone = fighter.Clone();
        Assert.Equal(fighter.Attack, clone.Attack);

        clone.TakeDamage(1);
        Assert.NotEqual(clone.Health, fighter.Health);
    }

    [Fact]
    public void ImmortalHorror_RevivesOnce_AliveAfterLethalDamage()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        Assert.True(horror.IsAlive);
    }

    [Fact]
    public void ImmortalHorror_RevivesOnce_HealthIsOneAfterRevive()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        Assert.True(horror.IsAlive);
        Assert.Equal(1, horror.Health);
    }

    [Fact]
    public void ImmortalHorror_RevivesOnce_DiesAfterSecondLethalHit()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        Assert.True(horror.IsAlive);
        Assert.Equal(1, horror.Health);

        horror.TakeDamage(10);
        Assert.False(horror.IsAlive);
    }

    [Fact]
    public void ImmortalHorror_RevivesOnce_HealthIsZeroAfterSecondLethalHit()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        Assert.True(horror.IsAlive);
        Assert.Equal(1, horror.Health);

        horror.TakeDamage(10);
        Assert.False(horror.IsAlive);
        Assert.Equal(0, horror.Health);
    }

    [Fact]
    public void ImmortalHorror_ClonePreservesFlag_OriginalRevivesOnce()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        Assert.True(horror.IsAlive);
        Assert.Equal(1, horror.Health);
    }

    [Fact]
    public void ImmortalHorror_ClonePreservesFlag_CloneDiesAfterLethalHit()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        ICreature clone = horror.Clone();
        clone.TakeDamage(10);

        Assert.False(clone.IsAlive);
    }

    [Fact]
    public void ImmortalHorror_ClonePreservesFlag_OriginalStateIsPreserved()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        ICreature clone = horror.Clone();
        clone.TakeDamage(10);

        Assert.True(horror.IsAlive);
    }

    [Fact]
    public void ImmortalHorror_ClonePreservesFlag_OriginalHealthIsOne()
    {
        ICreature horror = ImmortalHorrorFactory.CreateCreature();
        horror.TakeDamage(4);

        ICreature clone = horror.Clone();
        clone.TakeDamage(10);
        Assert.False(clone.IsAlive);

        Assert.True(horror.IsAlive);
        Assert.Equal(1, horror.Health);
    }

    [Fact]
    public void MimikCase_CopiesMaxStatsOnAttack_AttackIsMaximal()
    {
        ICreature mimik = MimikCaseFactory.CreateCreature();
        var big = new TestCreature("Big", 5, 10);

        mimik.AttackCreature(big);
        Assert.True(mimik.Attack >= 5);
    }

    [Fact]
    public void MimikCase_CopiesMaxStatsOnAttack_HealthIsMaximal()
    {
        ICreature mimik = MimikCaseFactory.CreateCreature();
        var big = new TestCreature("Big", 5, 10);

        mimik.AttackCreature(big);

        Assert.True(mimik.Attack >= 5);
        Assert.True(mimik.Health >= 10);
    }

    [Fact]
    public void MimikCase_ClonePreservesStats_CloneAttackMatchesOriginal()
    {
        ICreature mimik = MimikCaseFactory.CreateCreature();
        var big = new TestCreature("Big", 5, 10);

        mimik.AttackCreature(big);
        ICreature clone = mimik.Clone();

        Assert.Equal(mimik.Attack, clone.Attack);
    }

    [Fact]
    public void MimikCase_ClonePreservesStats_CloneHealthMatchesOriginal()
    {
        ICreature mimik = MimikCaseFactory.CreateCreature();
        var big = new TestCreature("Big", 5, 10);

        mimik.AttackCreature(big);
        ICreature clone = mimik.Clone();

        Assert.Equal(mimik.Attack, clone.Attack);
        Assert.Equal(mimik.Health, clone.Health);
    }

    [Fact]
    public void MimikCase_ClonePreservesStats_CloneAttacksAndStateChanges()
    {
        ICreature mimik = MimikCaseFactory.CreateCreature();
        var big = new TestCreature("Big", 5, 10);
        mimik.AttackCreature(big);
        ICreature clone = mimik.Clone();
        var small = new TestCreature("Small", 1, 1);
        clone.AttackCreature(small);

        Assert.Equal(mimik.Attack, clone.Attack);
        Assert.Equal(mimik.Health, clone.Health);
        Assert.Equal(clone.Health, mimik.Health);
    }

    [Fact]
    public void MimikCase_ClonePreservesStats_CloneIsIndependent()
    {
        ICreature mimik = MimikCaseFactory.CreateCreature();
        var big = new TestCreature("Big", 5, 10);

        mimik.AttackCreature(big);
        ICreature clone = mimik.Clone();

        Assert.Equal(mimik.Attack, clone.Attack);
        Assert.Equal(mimik.Health, clone.Health);

        var small = new TestCreature("Small", 1, 1);
        clone.AttackCreature(small);

        Assert.Equal(clone.Health, mimik.Health);
    }

    [Fact]
    public void AmuletMaster_AttackPerformsDoubleAttack_EnemyHealthIsTwo()
    {
        ICreature am = AmuletMasterFactory.CreateCreature();
        var enemy = new TestCreature("Enemy", 1, 12);

        am.AttackCreature(enemy);

        Assert.Equal(2, enemy.Health);
    }

    [Fact]
    public void AmuletMaster_TakeTwoLargeHits_DiesAfterSecondHitIsNotAlive()
    {
        ICreature am = AmuletMasterFactory.CreateCreature();
        am.TakeDamage(5);
        am.TakeDamage(5);

        Assert.False(am.IsAlive);
    }

    [Fact]
    public void AmuletMaster_TakeTwoLargeHits_HealthIsZero()
    {
        ICreature am = AmuletMasterFactory.CreateCreature();
        am.TakeDamage(5);
        am.TakeDamage(5);

        Assert.False(am.IsAlive);
        Assert.Equal(0, am.Health);
    }

    [Fact]
    public void AmuletMaster_Clone_IndependenceOfState()
    {
        ICreature am = AmuletMasterFactory.CreateCreature();
        am.TakeDamage(5);

        ICreature clone = am.Clone();
        clone.TakeDamage(5);
        Assert.NotEqual(am.Health, clone.Health);
    }
}
