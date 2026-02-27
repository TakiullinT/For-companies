namespace Timescale.Api.Domain.Entities;

public class ValueEntity
{
    public long Id { get; set; }
    public int FileId { get; set; }
    
    public DateTime Date { get; set; }
    public double ExecutionTime { get; set; }
    public double Value { get; set; }

    public virtual FileEntity File { get; set; } = null!;
}