using discipline.application.Domain.Entities;
using discipline.application.Domain.ValueObjects.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.application.Infrastructure.DAL.EntitiesConfiguration;

internal sealed class ActivityTypeConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new(y))
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasConversion(x => x.Value, y => new(y))
            .IsRequired();

        builder
            .Property(x => x.IsChecked)
            .HasConversion(x => x.Value, y => new(y))
            .IsRequired();

        builder
            .Property(x => x.ParentRuleId)
            .HasConversion(x => x.Value, y => new(y));
    }
}