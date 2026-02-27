namespace Timescale.Api.Application.DTOs;

public class ValueResponseDTO
{
    public DateTime Date { get; set; }
    public double ExecutionTime { get; set; }
    public double Value { get; set; }
}