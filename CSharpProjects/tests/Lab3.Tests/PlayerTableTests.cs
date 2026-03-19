using Itmo.ObjectOrientedProgramming.Lab3.PlayerTableInfo;
using Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;
using Itmo.ObjectOrientedProgramming.Lab3.Tests.Mocks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class PlayerTableTests
{
    [Fact]
    public void AddCreature_StoreClone_ResultIsSuccess()
    {
        var original = new TestCreature("C", 2, 5);
        var table = new PlayerTable();

        Result result = table.AddCreature(original);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void AddCreature_StoreClone_TableContainsOneCreature()
    {
        var original = new TestCreature("C", 2, 5);
        var table = new PlayerTable();

        Result result = table.AddCreature(original);

        Assert.True(result.IsSuccess);
        Assert.Single(table.Creatures);
    }

    [Fact]
    public void AddCreature_StoreClone_ReferenceIsNotSame()
    {
        var original = new TestCreature("C", 2, 5);
        var table = new PlayerTable();

        Result result = table.AddCreature(original);

        Assert.True(result.IsSuccess);
        Assert.Single(table.Creatures);
        Assert.NotSame(original, table.Creatures.First());
    }

    [Fact]
    public void AddCreature_ShouldFailWhenNull_ResultIsFailure()
    {
        var table = new PlayerTable();

        Result result = table.AddCreature(null);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void AddCreature_ShouldFailWhenNull_TableIsEmpty()
    {
        var table = new PlayerTable();

        Result result = table.AddCreature(null);

        Assert.False(result.IsSuccess);
        Assert.Empty(table.Creatures);
    }

    [Fact]
    public void AddCreature_ShouldFailWhenMoreThanSeven_ExtraAddFails()
    {
        var table = new PlayerTable();

        for (int i = 0; i < 7; i++)
        {
            Result result1 = table.AddCreature(new TestCreature("C" + i, 1, 1));
            Assert.True(result1.IsSuccess);
        }

        Result result = table.AddCreature(new TestCreature("Extra", 1, 1));
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void AddCreature_ShouldFailWhenMoreThanSeven_TableContainsExactlySeven()
    {
        var table = new PlayerTable();

        for (int i = 0; i < 7; i++)
        {
            Result result1 = table.AddCreature(new TestCreature("C" + i, 1, 1));
            Assert.True(result1.IsSuccess);
        }

        Result result = table.AddCreature(new TestCreature("Extra", 1, 1));
        Assert.False(result.IsSuccess);
        Assert.Equal(7, table.Creatures.Count);
    }

    [Fact]
    public void GetAttackers_ReturnsOnlyAlive_AliveCreatureIsIncluded()
    {
        var table = new PlayerTable();

        var alive = new TestCreature("Alive", 1, 3);
        var dead = new TestCreature("Dead", 1, 2);
        dead.TakeDamage(5);

        table.AddCreature(alive);
        table.AddCreature(dead);

        var attackers = table.GetAttackers().ToList();

        Assert.Contains(attackers, c => c.Name == "Alive");
    }

    [Fact]
    public void GetAttackers_ReturnsOnlyAlive_DeadCreatureIsNotIncluded()
    {
        var table = new PlayerTable();

        var alive = new TestCreature("Alive", 1, 3);
        var dead = new TestCreature("Dead", 1, 2);
        dead.TakeDamage(5);

        table.AddCreature(alive);
        table.AddCreature(dead);

        var attackers = table.GetAttackers().ToList();

        Assert.Contains(attackers, c => c.Name == "Alive");
        Assert.DoesNotContain(attackers, c => c.Name == "Dead");
    }

    [Fact]
    public void GetDefenders_ReturnsOnlyAlive_AliveCreatureIsIncluded()
    {
        var table = new PlayerTable();

        var alive = new TestCreature("Alive", 1, 3);
        var dead = new TestCreature("Dead", 1, 2);
        dead.TakeDamage(10);

        table.AddCreature(alive);
        table.AddCreature(dead);

        var defenders = table.GetDefenders().ToList();

        Assert.Contains(defenders, c => c.Name == "Alive");
    }

    [Fact]
    public void GetDefenders_ReturnsOnlyAlive_DeadCreatureIsNotIncluded()
    {
        var table = new PlayerTable();

        var alive = new TestCreature("Alive", 1, 3);
        var dead = new TestCreature("Dead", 1, 2);
        dead.TakeDamage(10);

        table.AddCreature(alive);
        table.AddCreature(dead);

        var defenders = table.GetDefenders().ToList();

        Assert.Contains(defenders, c => c.Name == "Alive");
        Assert.DoesNotContain(defenders, c => c.Name == "Dead");
    }
}