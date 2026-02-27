using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timescale.Api.Domain.Entities;

namespace Timescale.Api.Infrastructure.Condigurations;

public class ResultCondiguration : IEntityTypeConfiguration<ResultEntity>
{
    public void Configure(EntityTypeBuilder<ResultEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(r => r.File)
            .WithOne(f => f.Result)
            .HasForeignKey<ResultEntity>(r => r.FileId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Property(x => x.AverageValue).IsRequired();
        builder.Property(x => x.MedianValue).IsRequired();
    }
}