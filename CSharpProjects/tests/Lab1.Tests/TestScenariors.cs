using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class TestScenariors
{
    [Fact]
    public void Scenario1_SuccessSpeedLessThanLimit()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 5000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 3000);
        (NormalSegment? segment2, SimulationResult result2) = NormalSegment.CreateNormalSegment(200);
        Assert.NotNull(segment1);
        Assert.NotNull(segment2);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(
            new List<RouteSegment>
            {
                segment1,
                segment2,
            },
            50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Scenario2_FailSpeedMoreThanLimit()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 3000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 5000);
        (NormalSegment? segment2, SimulationResult result2) = NormalSegment.CreateNormalSegment(200);
        Assert.NotNull(segment1);
        Assert.NotNull(segment2);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new List<RouteSegment> { segment1, segment2 }, 50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario3_PowerSegmentAndLimitSpeed()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 5000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 3000);
        (NormalSegment? segment2, SimulationResult result2) = NormalSegment.CreateNormalSegment(200);
        (StationSegment? segment3, SimulationResult result3) = StationSegment.CreateStationSegment(50, 2, 2, 50);
        (NormalSegment? segment4, SimulationResult result4) = NormalSegment.CreateNormalSegment(100);

        Assert.NotNull(segment1);
        Assert.NotNull(segment2);
        Assert.NotNull(segment3);
        Assert.NotNull(segment4);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new RouteSegment[] { segment1, segment2, segment3, segment4 }, 50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Scenario4_FailSpeedAtStation()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 5000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 10000);
        (StationSegment? segment2, SimulationResult result2) = StationSegment.CreateStationSegment(50, 5, 5, 50);
        (NormalSegment? segment3, SimulationResult result3) = NormalSegment.CreateNormalSegment(100);

        Assert.NotNull(segment1);
        Assert.NotNull(segment2);
        Assert.NotNull(segment3);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new List<RouteSegment> { segment1, segment2, segment3 }, 50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario5_FailSpeedAtRouteEnd()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 5000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 3000);
        (NormalSegment? segment2, SimulationResult result2) = NormalSegment.CreateNormalSegment(200);
        (StationSegment? segment3, SimulationResult result3) = StationSegment.CreateStationSegment(50, 5, 5, 50);
        (NormalSegment? segment4, SimulationResult result4) = NormalSegment.CreateNormalSegment(100);

        Assert.NotNull(segment1);
        Assert.NotNull(segment2);
        Assert.NotNull(segment3);
        Assert.NotNull(segment4);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new List<RouteSegment> { segment1, segment2, segment3, segment4 }, 20);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario6_SuccessDifferentSegments()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 10000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 5000);
        (NormalSegment? segment2, SimulationResult result2) = NormalSegment.CreateNormalSegment(100);
        (PowerSegment? segment3, SimulationResult result3) = PowerSegment.CreatePowerSegment(50, -4000);
        (StationSegment? segment4, SimulationResult result4) = StationSegment.CreateStationSegment(50, 5, 5, 50);
        (NormalSegment? segment5, SimulationResult result5) = NormalSegment.CreateNormalSegment(50);
        (PowerSegment? segment6, SimulationResult result6) = PowerSegment.CreatePowerSegment(50, 5000);
        (NormalSegment? segment7, SimulationResult result7) = NormalSegment.CreateNormalSegment(50);
        (PowerSegment? segment8, SimulationResult result8) = PowerSegment.CreatePowerSegment(50, -5000);

        Assert.NotNull(segment1);
        Assert.NotNull(segment2);
        Assert.NotNull(segment3);
        Assert.NotNull(segment4);
        Assert.NotNull(segment5);
        Assert.NotNull(segment6);
        Assert.NotNull(segment7);
        Assert.NotNull(segment8);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new List<RouteSegment> { segment1, segment2, segment3, segment4, segment5, segment6, segment7, segment8 }, 50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Scenario7_FailNormalRoute()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 5000, 0.1);
        Assert.NotNull(train);

        (NormalSegment? segment, SimulationResult result1) = NormalSegment.CreateNormalSegment(100);
        Assert.NotNull(segment);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new RouteSegment[] { segment }, 50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario8_FailApplyMorePowerThanAllowed()
    {
        (Train? train, SimulationResult trainResultCreation) = Train.CreateTrain(1000, 5000, 0.1);
        Assert.NotNull(train);

        (PowerSegment? segment1, SimulationResult result1) = PowerSegment.CreatePowerSegment(100, 3000);
        (PowerSegment? segment2, SimulationResult result2) = PowerSegment.CreatePowerSegment(100, -6000);
        Assert.NotNull(segment1);
        Assert.NotNull(segment2);

        (Route? route, SimulationResult routeResultCreation) = Route.CreateRoute(new RouteSegment[] { segment1, segment2 }, 50);
        Assert.NotNull(route);

        SimulationResult result = route.TraverseRoute(train);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario9_FailWithInvalidTimes()
    {
        (StationSegment? segment1, SimulationResult result1) = StationSegment.CreateStationSegment(50, 0, 0, 50);
        (StationSegment? segment2, SimulationResult result2) = StationSegment.CreateStationSegment(50, -1, 5, 50);
        (StationSegment? segment3, SimulationResult result3) = StationSegment.CreateStationSegment(50, 2, -3, 50);

        Assert.Null(segment1);
        Assert.False(result1.IsSuccess);
        Assert.Null(segment2);
        Assert.False(result2.IsSuccess);
        Assert.Null(segment3);
        Assert.False(result3.IsSuccess);
    }

    [Fact]
    public void Scenario10_CalculatesDistanceTimeCorrectly()
    {
        (Train? train, SimulationResult? trainResultCreation) = Train.CreateTrain(1000, 5000, 0.5);
        Assert.NotNull(train);

        train.ApplyPower(1000);

        SimulationResult result = train.CalculateDistance(10);
        Assert.True(result.IsSuccess);
        Assert.InRange(result.TimeTaken, 4.2, 4.3);
    }
}
