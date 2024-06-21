using discipline.application.Domain.Entities;
using discipline.application.Domain.ValueObjects.DailyProductivity;
using discipline.application.Domain.ValueObjects.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.application.Infrastructure.DAL.EntitiesConfiguration;

internal sealed class DailyProductivityTypeConfiguration : IEntityTypeConfiguration<DailyProductivity>
{
    public void Configure(EntityTypeBuilder<DailyProductivity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, y => new EntityId(y))
            .IsRequired();

        builder
            .Property(x => x.Day)
            .HasConversion(x => x.Value, y => new Day(y))
            .IsRequired();

        builder
            .HasMany<Activity>(x => x.ActivityItems)
            .WithOne();

        builder
            .HasIndex(x => x.Day)
            .IsUnique();
    }
}