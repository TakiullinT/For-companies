namespace Timescale.Api.Application.DTOs;

public record ResultResponseDTO
(
    string FileName,
    double TimeDelta,
    DateTime FirstOperationRun,
    double AverageExecutionTime,
    double AverageValue,
    double MedianValue,
    double MaxValue,
    double MinValue 
    );