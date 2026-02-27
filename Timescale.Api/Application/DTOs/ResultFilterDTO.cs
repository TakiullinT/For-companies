namespace Timescale.Api.Application.DTOs;

public class ResultFilterDTO
{
    public string? FileName { get; set; }

    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }

    public double? MinAverageValue { get; set; }
    public double? MaxAverageValue { get; set; }

    public double? MinAverageExecutionTime { get; set; }
    public double? MaxAverageExecutionTime { get; set; }
}