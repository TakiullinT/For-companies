namespace Itmo.ObjectOrientedProgramming.Lab1;

public class StationSegment : RouteSegment
{
    public double BoardingTime { get; }

    public double LeavingTime { get; }

    public double LimitSpeed { get; }

    private StationSegment(double segmentLength, double boardingTime, double leavingTime, double limitSpeed) : base(segmentLength)
    {
        BoardingTime = boardingTime;
        LeavingTime = leavingTime;
        LimitSpeed = limitSpeed;
    }

    public static (StationSegment? Segment, SimulationResult Result) CreateStationSegment(double segmentLength, double boardingTime, double leavingTime, double limitSpeed)
    {
        if (boardingTime <= 0 || leavingTime <= 0) return (null, SimulationResult.Fail(0, "Время посадки и высадки должно быть > 0"));
        if (limitSpeed <= 0) return (null, SimulationResult.Fail(0, "Лимит скорости должен быть > 0"));
        if (segmentLength <= 0) return (null, SimulationResult.Fail(0, "Длина должна быть > 0"));

        var segment = new StationSegment(segmentLength, boardingTime, leavingTime, limitSpeed);
        return (segment, SimulationResult.Success(0, "Станция успешно создана"));
    }

    public override SimulationResult Traverse(Train train)
    {
        SimulationResult pathResult = train.CalculateDistance(SegmentLength);
        if (!pathResult.IsSuccess) return pathResult;

        double arrivalSpeed = train.Speed;
        if (arrivalSpeed > LimitSpeed) return SimulationResult.Fail(pathResult.TimeTaken, "Скорость для станции ну очень высока");

        train.StopTrain();
        train.RestoreTrainSpeed(arrivalSpeed);

        double totalTime = BoardingTime + LeavingTime + pathResult.TimeTaken;

        return SimulationResult.Success(totalTime, "Мы доехали до станции");
    }
}
