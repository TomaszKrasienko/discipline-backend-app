using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Mappers;

internal static class StageMappingExtensions
{
    internal static StageDocument MapAsDocument(this Stage stage)
        => new ()
        {
            StageId = stage.Id.ToString(),
            Title = stage.Title,
            Index = stage.Index,
            IsChecked = stage.IsChecked
        };
}