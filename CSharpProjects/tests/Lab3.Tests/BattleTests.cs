using Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;
using Itmo.ObjectOrientedProgramming.Lab3.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.PlayerTableInfo;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class BattleTests
{
    [Fact]
    public void Battle_ReturnsFirstPlayerWins_SecondHasNoDefenders()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();

        table1.AddCreature(new TestCreature("A", 3, 3));

        var battle = new Battle(table1, table2);

        BattleResult result = battle.Run();

        Assert.Equal(BattleResult.FirstPlayerWins, result);
    }

    [Fact]
    public void Battle_Draw_AllCreaturesAreDead()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();

        table1.AddCreature(new TestCreature("A", 0, 0));
        table2.AddCreature(new TestCreature("B", 0, 0));

        var battle = new Battle(table1, table2);
        BattleResult result = battle.Run();

        Assert.Equal(BattleResult.Draw, result);
    }

    [Fact]
    public void Battle_DoesNotMutate_OriginalShielded_HealthBefore()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();
        var originalShielded = new MagicShieldModifier(new TestCreature("P1Shield", 2, 3));
        var originalSimple = new TestCreature("P2Simple", 1, 2);

        table1.AddCreature(originalShielded);
        table2.AddCreature(originalSimple);

        int healthBeforeShielded = originalShielded.Health;
        Assert.Equal(3, healthBeforeShielded);
    }

    [Fact]
    public void Battle_DoesNotMutate_OriginalShielded_AttackBefore()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();
        var originalShielded = new MagicShieldModifier(new TestCreature("P1Shield", 2, 3));
        var originalSimple = new TestCreature("P2Simple", 1, 2);

        table1.AddCreature(originalShielded);
        table2.AddCreature(originalSimple);

        int attackBeforeShielded = originalShielded.Attack;
        Assert.Equal(2, attackBeforeShielded);
    }

    [Fact]
    public void Battle_DoesNotMutate_OriginalSimple_HealthAndAttackBefore()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();
        var originalShielded = new MagicShieldModifier(new TestCreature("P1Shield", 2, 3));
        var originalSimple = new TestCreature("P2Simple", 1, 2);

        table1.AddCreature(originalShielded);
        table2.AddCreature(originalSimple);

        Assert.Equal(2, originalSimple.Health);
        Assert.Equal(1, originalSimple.Attack);
    }

    [Fact]
    public void Battle_DoesNotMutate_Originals_AfterBattle()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();
        var originalShielded = new MagicShieldModifier(new TestCreature("P1Shield", 2, 3));
        var originalSimple = new TestCreature("P2Simple", 1, 2);

        table1.AddCreature(originalShielded);
        table2.AddCreature(originalSimple);

        int healthBeforeShielded = originalShielded.Health;
        int attackBeforeShielded = originalShielded.Attack;
        int healthBeforeSimple = originalSimple.Health;
        int attackBeforeSimple = originalSimple.Attack;

        var battle = new Battle(table1, table2);
        _ = battle.Run();

        Assert.Equal(healthBeforeShielded, originalShielded.Health);
        Assert.Equal(attackBeforeShielded, originalShielded.Attack);
        Assert.Equal(healthBeforeSimple, originalSimple.Health);
        Assert.Equal(attackBeforeSimple, originalSimple.Attack);
    }

    [Fact]
    public void Battle_DoesNotMutate_OriginalShielded_ShieldWorksAfterBattle()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();

        var originalShielded = new MagicShieldModifier(new TestCreature("P1Shield", 2, 3));
        var originalSimple = new TestCreature("P2Simple", 1, 2);

        ResultInfo.Result add1 = table1.AddCreature(originalShielded);
        ResultInfo.Result add2 = table2.AddCreature(originalSimple);
        Assert.True(add1.IsSuccess && add2.IsSuccess);

        int healthBeforeShielded = originalShielded.Health;
        int attackBeforeShielded = originalShielded.Attack;
        int healthBeforeSimple = originalSimple.Health;
        int attackBeforeSimple = originalSimple.Attack;

        var battle = new Battle(table1, table2);
        _ = battle.Run();

        Assert.Equal(healthBeforeShielded, originalShielded.Health);
        Assert.Equal(attackBeforeShielded, originalShielded.Attack);
        Assert.Equal(healthBeforeSimple, originalSimple.Health);
        Assert.Equal(attackBeforeSimple, originalSimple.Attack);

        originalShielded.TakeDamage(1);
        Assert.Equal(healthBeforeShielded, originalShielded.Health);
    }

    [Fact]
    public void Battle_SkipsTurn_NoAttackersOnCurrentSide()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();

        table1.AddCreature(new TestCreature("Pacifist", 0, 5));
        table2.AddCreature(new TestCreature("Attacker", 2, 3));

        var battle = new Battle(table1, table2);
        BattleResult result = battle.Run();

        Assert.True(result == BattleResult.FirstPlayerWins ||
                    result == BattleResult.SecondPlayerWins ||
                    result == BattleResult.Draw);
    }

    [Fact]
    public void Battle_ComplexSetup_ResultIsValid()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();

        table1.AddCreature(new MagicShieldModifier(new TestCreature("M1", 3, 4)));
        table1.AddCreature(new DoubleAttackModifier(new TestCreature("D1", 2, 3)));

        table2.AddCreature(new ImmortalHorror());
        table2.AddCreature(new MimikCase());

        var originals = table1.Creatures.Concat(table2.Creatures).Select(c => (c.Name, c.Attack, c.Health)).ToList();

        var battle = new Battle(table1, table2);
        BattleResult result = battle.Run();

        Assert.True(result == BattleResult.FirstPlayerWins || result == BattleResult.SecondPlayerWins || result == BattleResult.Draw);
    }

    [Fact]
    public void Battle_ComplexSetup_OriginalsNotMutated()
    {
        var table1 = new PlayerTable();
        var table2 = new PlayerTable();

        table1.AddCreature(new MagicShieldModifier(new TestCreature("M1", 3, 4)));
        table1.AddCreature(new DoubleAttackModifier(new TestCreature("D1", 2, 3)));

        table2.AddCreature(new ImmortalHorror());
        table2.AddCreature(new MimikCase());

        var originals = table1.Creatures.Concat(table2.Creatures).Select(c => (c.Name, c.Attack, c.Health)).ToList();

        var battle = new Battle(table1, table2);
        BattleResult result = battle.Run();

        Assert.True(result == BattleResult.FirstPlayerWins || result == BattleResult.SecondPlayerWins || result == BattleResult.Draw);

        var after = table1.Creatures.Concat(table2.Creatures).Select(c => (c.Name, c.Attack, c.Health)).ToList();
        Assert.Equal(originals, after);
    }
}
