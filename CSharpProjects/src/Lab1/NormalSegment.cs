namespace Itmo.ObjectOrientedProgramming.Lab1;

public class NormalSegment : RouteSegment
{
    private NormalSegment(double segmentLength) : base(segmentLength) { }

    public static (NormalSegment? Segment, SimulationResult Result) CreateNormalSegment(double segmentLength)
    {
        if (segmentLength <= 0) return (null, SimulationResult.Fail(0, "Длина должна быть > 0"));

        var segment = new NormalSegment(segmentLength);
        return (segment, SimulationResult.Success(0, "Обычный сегмент успешно создан"));
    }

    public override SimulationResult Traverse(Train train)
    {
        return train.CalculateDistance(SegmentLength);
    }
}
