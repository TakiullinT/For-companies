using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timescale.Api.Domain.Entities;

namespace Timescale.Api.Infrastructure.Condigurations;

public class ValueConfiguration : IEntityTypeConfiguration<ValueEntity>
{
    public void Configure(EntityTypeBuilder<ValueEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.ExecutionTime).IsRequired();
        
        builder.HasOne(v => v.File)
            .WithMany(f => f.Values)
            .HasForeignKey(v => v.FileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}