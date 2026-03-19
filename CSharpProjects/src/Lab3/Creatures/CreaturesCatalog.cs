using Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab3.Creatures;

public class CreaturesCatalog
{
    private Dictionary<string, ICreature> Catalog { get; } = new();

    public void Register(string name, ICreature creature)
    {
        if (string.IsNullOrEmpty(name)) return;
        if (creature is null) return;
        Catalog[name] = creature;
    }

    public ResultType<ICreature> Create(string name)
    {
        if (!Catalog.ContainsKey(name)) return ResultType<ICreature>.Fail("Каталог не содержит нужного ключа");

        return ResultType<ICreature>.Success(Catalog[name].Clone());
    }
}