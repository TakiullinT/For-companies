namespace Itmo.ObjectOrientedProgramming.Lab1;

public record SimulationResult(bool IsSuccess, double TimeTaken, string Message = "")
{
    public static SimulationResult Fail(double timeTaken, string message = "") => new(false, timeTaken, message);

    public static SimulationResult Success(double timeTaken, string message = "") => new(true, timeTaken, message);
}
