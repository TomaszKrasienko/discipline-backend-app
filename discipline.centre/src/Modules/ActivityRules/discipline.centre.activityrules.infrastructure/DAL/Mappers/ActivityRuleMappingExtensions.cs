using discipline.centre.activityrules.infrastructure.DAL.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.domain;

internal static class ActivityRuleMappingExtensions
{
    internal static ActivityRuleDocument MapAsDocument(this ActivityRule entity)
        => new()
        {
            Id = entity.Id.Value.ToString(),
            UserId = entity.UserId.ToString(),
            Title = entity.Title,
            Mode = entity.Mode,
            SelectedDays = entity.SelectedDays?.Select(x => x.Value).ToList()
        };
}