using discipline.application.Domain.Entities;
using discipline.application.Domain.ValueObjects.ActivityRules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.application.Infrastructure.DAL.EntitiesConfiguration;

internal sealed class ActivityRuleTypeConfiguration : IEntityTypeConfiguration<ActivityRule>
{
    public void Configure(EntityTypeBuilder<ActivityRule> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, y => new(y))
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasConversion(x => x.Value, y => new(y))
            .HasMaxLength(40)
            .IsRequired();

        builder
            .Property(x => x.Mode)
            .HasConversion(x => x.Value, y => new(y))
            .IsRequired();

        builder
            .Property(x => x.SelectedDays)
            .HasConversion(x => string.Join(",", x.Select(y => y.Value).ToList()),
                y => string.IsNullOrWhiteSpace(y) ? null : y.Split(",", StringSplitOptions.None)
                    .Select(day => new SelectedDay(int.Parse(day))).ToList());
        
        builder
            .Property(x => x.SelectedDays).Metadata.SetValueComparer(
                new ValueComparer<IReadOnlyCollection<SelectedDay>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));

        builder
            .HasIndex(x => x.Title)
            .IsUnique();
    }
}