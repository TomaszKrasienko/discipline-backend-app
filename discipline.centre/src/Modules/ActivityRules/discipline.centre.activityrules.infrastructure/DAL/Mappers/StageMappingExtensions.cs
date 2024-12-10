using discipline.centre.activityrules.infrastructure.DAL.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.domain;

internal static class StageMappingExtensions
{
    internal static StageDocument MapAsDocument(this Stage stage)
        => new ()
        {
            StageId = stage.Id.ToString(),
            Title = stage.Title,
            Index = stage.Index
        };
}