namespace Itmo.ObjectOrientedProgramming.Lab1;

public class Train
{
    public double Mass { get; }

    public double MaxPower { get; }

    public double TimeStep { get; }

    public double Speed { get; private set; } = 0;

    public double Acceleration { get; private set; } = 0;

    private Train(double mass, double maxPower, double timeStep)
    {
        Mass = mass;
        MaxPower = maxPower;
        TimeStep = timeStep;
        Speed = 0;
        Acceleration = 0;
    }

    public static (Train? Train, SimulationResult Result) CreateTrain(double mass, double maxPower, double timeStep)
    {
        if (mass <= 0) return (null, SimulationResult.Fail(0, "Масса поезда должна быть положительной"));
        if (maxPower <= 0) return (null, SimulationResult.Fail(0, "Макс сила должна быть положительной"));
        if (timeStep <= 0) return (null, SimulationResult.Fail(0, "Шаг времени должен быть положительным"));

        return (new Train(mass, maxPower, timeStep), SimulationResult.Success(0, "Поезд успешно создан"));
    }

    public void StopTrain()
    {
        Speed = 0;
        Acceleration = 0;
    }

    public SimulationResult RestoreTrainSpeed(double speed)
    {
        if (speed < 0) return SimulationResult.Fail(0, "Нельзя установить отрицательную скорость");

        Speed = speed;
        Acceleration = 0;
        return SimulationResult.Success(0, "Скорость восстановлена");
    }

    public SimulationResult ApplyPower(double power)
    {
        if (Math.Abs(power) > MaxPower) return SimulationResult.Fail(0, "Сила превысила допустимую");

        Acceleration = power / Mass;
        return SimulationResult.Success(0, "Сила применена");
    }

    public SimulationResult CalculateDistance(double distance)
    {
        if (distance <= 0) return SimulationResult.Fail(0, "Дистанция должна быть положительной");

        double remainingDistance = distance;
        double totalTime = 0;

        while (remainingDistance > 0)
        {
            if (Speed <= 0 && Acceleration <= 0) return SimulationResult.Fail(totalTime, "Поезд не может двигаться");
            double fullStepNewSpeed = Speed + (Acceleration * TimeStep);

            if (fullStepNewSpeed <= 0) return SimulationResult.Fail(totalTime, "Поезд остановился из-за отрицательной скорости");

            double fullStepDistance = fullStepNewSpeed * TimeStep;

            if (fullStepDistance >= remainingDistance)
            {
                double timeNeeded = remainingDistance / fullStepNewSpeed;
                totalTime += timeNeeded;
                Speed = Speed + (Acceleration * timeNeeded);
                remainingDistance = 0;
            }
            else
            {
                remainingDistance -= fullStepDistance;
                totalTime += TimeStep;
                Speed = fullStepNewSpeed;
            }
        }

        return SimulationResult.Success(totalTime, "Дистанция успешно пройдена");
    }
}
