namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures;

public class CreatureBuilder
{
    public int Attack { get; private set; } = 1;

    public int Health { get; private set; } = 1;

    public string Name { get; private set; }

    private List<Func<ICreature, ICreature>> Modifiers { get; } = new();

    private Func<string, int, int, Creature> Factory { get; }

    public CreatureBuilder(Func<string, int, int, Creature> factory, string name = "Creature")
    {
        Factory = factory;
        Name = name;
    }

    public CreatureBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CreatureBuilder WithStats(int attack, int health)
    {
        Attack = attack;
        Health = health;
        return this;
    }

    public CreatureBuilder WithModifier(Func<ICreature, ICreature> modifier)
    {
        Modifiers.Add(modifier);
        return this;
    }

    public ICreature Build()
    {
        Creature baseCreature = Factory(Name, Attack, Health);
        ICreature result = baseCreature;

        foreach (Func<ICreature, ICreature> modifier in Modifiers)
        {
            result = modifier(result);
        }

        return result;
    }
}