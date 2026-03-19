namespace Itmo.ObjectOrientedProgramming.Lab1;

public class Route
{
    public IReadOnlyList<RouteSegment> Segments { get; }

    public double LimitEndSpeed { get; }

    private Route(IReadOnlyList<RouteSegment> segments, double limitEndSpeed)
    {
        Segments = segments;
        LimitEndSpeed = limitEndSpeed;
    }

    public static (Route? Route, SimulationResult Result) CreateRoute(IReadOnlyList<RouteSegment> segments, double limitEndSpeed)
    {
        if (segments == null || segments.Count == 0) return (null, SimulationResult.Fail(0, "Маршрут не может быть пустым"));
        if (limitEndSpeed < 0) return (null, SimulationResult.Fail(0, "Макс скорость должна быть неотрицательной"));

        return (new Route(segments, limitEndSpeed), SimulationResult.Success(0, "Маршрут создан"));
    }

    public SimulationResult TraverseRoute(Train train)
    {
        double totalTime = 0;

        foreach (RouteSegment segment in Segments)
        {
            SimulationResult result = segment.Traverse(train);
            if (!result.IsSuccess) return SimulationResult.Fail(totalTime + result.TimeTaken, "Неудача на сегменте");

            totalTime += result.TimeTaken;
        }

        if (train.Speed > LimitEndSpeed) return SimulationResult.Fail(totalTime, "Превышена макс скорость в конце");

        train.StopTrain();
        return SimulationResult.Success(totalTime, "Удачно доехали до конца");
    }
}
