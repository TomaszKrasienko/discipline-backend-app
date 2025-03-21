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
            Title = entity.Details.Title,
            Note = entity.Details.Note,
            Mode = entity.Mode,
            SelectedDays = entity.SelectedDays?.Values.Select(x => (int)x),
            Stages = entity.Stages?.Select(x => x.MapAsDocument())
        };
}