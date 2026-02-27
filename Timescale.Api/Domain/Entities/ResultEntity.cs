namespace Timescale.Api.Domain.Entities;

public class ResultEntity
{
    public int Id { get; set; }
    public int FileId { get; set; }
    
    public double TimeDelta { get; set; }
    public DateTime FirstOperationRun { get; set; }
    public double AverageExecutionTime { get; set; }
    public double AverageValue { get; set; }
    public double MedianValue { get; set; }
    public double MaxValue { get; set; }
    public double MinValue { get; set; }

    public virtual FileEntity File { get; set; } = null!;
}