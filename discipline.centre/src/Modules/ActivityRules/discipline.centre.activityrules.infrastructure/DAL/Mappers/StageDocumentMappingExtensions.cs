using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal static class StageDocumentMappingExtensions
{
    internal static Stage MapAsEntity(this StageDocument document)
        => new (StageId.Parse(document.StageId), document.Title, document.Index);

    internal static StageDto MapAsDto(this StageDocument document)
        => new ()
        {
            StageId = StageId.Parse(document.StageId),
            Title = document.Title,
            Index = document.Index,
        };
}