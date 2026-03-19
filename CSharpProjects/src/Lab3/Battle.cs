using Itmo.ObjectOrientedProgramming.Lab3.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.PlayerTableInfo;
using System.Security.Cryptography;

namespace Itmo.ObjectOrientedProgramming.Lab3;

public enum BattleResult
{
    FirstPlayerWins,
    SecondPlayerWins,
    Draw,
}

public class Battle
{
    private IPlayerTable FirstPlayerTable { get; }

    private IPlayerTable SecondPlayerTable { get; }

    public Battle(IPlayerTable firstPlayerTable, IPlayerTable secondPlayerTable)
    {
        FirstPlayerTable = firstPlayerTable;
        SecondPlayerTable = secondPlayerTable;
    }

    public BattleResult Run()
    {
        var first = FirstPlayerTable.Creatures.Select(creature => creature.Clone()).ToList();
        var second = SecondPlayerTable.Creatures.Select(creature => creature.Clone()).ToList();

        bool firstPlayerTurn = true;

        while (true)
        {
            List<ICreature> attackers = firstPlayerTurn ? first : second;
            List<ICreature> defenders = firstPlayerTurn ? second : first;

            var aliveAttackers = attackers.Where(creature => creature.IsAlive && creature.Attack > 0).ToList();
            var aliveDefenders = defenders.Where(creature => creature.IsAlive).ToList();

            if (aliveAttackers.Count == 0 && aliveDefenders.Count == 0)
            {
                return BattleResult.Draw;
            }

            if (aliveAttackers.Count == 0)
            {
                firstPlayerTurn = !firstPlayerTurn;
                continue;
            }

            if (aliveDefenders.Count == 0)
            {
                return firstPlayerTurn ? BattleResult.FirstPlayerWins : BattleResult.SecondPlayerWins;
            }

            ICreature attacker = aliveAttackers[GetRandomIndex(aliveAttackers.Count)];
            ICreature defender = aliveDefenders[GetRandomIndex(aliveDefenders.Count)];

            attacker.AttackCreature(defender);

            firstPlayerTurn = !firstPlayerTurn;
        }
    }

    private int GetRandomIndex(int max)
    {
        return RandomNumberGenerator.GetInt32(max);
    }
}
