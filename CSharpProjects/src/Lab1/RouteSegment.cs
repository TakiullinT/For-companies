namespace Itmo.ObjectOrientedProgramming.Lab1;

public abstract class RouteSegment
{
    public double SegmentLength { get; }

    protected RouteSegment(double segmentLength)
    {
        SegmentLength = segmentLength >= 0 ? segmentLength : 0;
    }

    public abstract SimulationResult Traverse(Train train);
}