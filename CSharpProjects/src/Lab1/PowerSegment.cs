namespace Itmo.ObjectOrientedProgramming.Lab1;

public class PowerSegment : RouteSegment
{
    public double AppliedPower { get; }

    private PowerSegment(double segmentLength, double appliedPower) : base(segmentLength)
    {
        AppliedPower = appliedPower;
    }

    public static (PowerSegment? Segment, SimulationResult Result) CreatePowerSegment(double segmentLength, double appliedPower)
    {
        if (segmentLength <= 0) return (null, SimulationResult.Fail(0, "Длина должна быть > 0"));

        var segment = new PowerSegment(segmentLength, appliedPower);
        return (segment, SimulationResult.Success(0, "Сегмент с силой успешно создан"));
    }

    public override SimulationResult Traverse(Train train)
    {
        SimulationResult result = train.ApplyPower(AppliedPower);
        if (!result.IsSuccess) return SimulationResult.Fail(0, "Сила слишком высока");

        return train.CalculateDistance(SegmentLength);
    }
}
