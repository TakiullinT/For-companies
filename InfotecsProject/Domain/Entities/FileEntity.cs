namespace Timescale.Api.Domain.Entities;

public class FileEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public virtual ResultEntity? Result { get; set; }
    public virtual ICollection<ValueEntity> Values { get; set; } = new List<ValueEntity>();
}