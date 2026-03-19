namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures.CreatureSpecies;

public class AmuletMaster : Creature
{
    public AmuletMaster() : base("Мастер амулетов", 5, 2) { }

    public override ICreature Clone()
    {
        var clone = new AmuletMaster();
        clone.Attack = Attack;
        clone.Health = Health;
        return clone;
    }
}