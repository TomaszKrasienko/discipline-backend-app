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
        builder.HasKey(x => x.Day);

        builder
            .Property(x => x.Day)
            .HasConversion(x => x.Value, y => new (y))
            .IsRequired();

        builder
            .HasMany<Activity>(x => x.Activities)
            .WithOne();

        builder
            .HasIndex(x => x.Day)
            .IsUnique();
    }
}