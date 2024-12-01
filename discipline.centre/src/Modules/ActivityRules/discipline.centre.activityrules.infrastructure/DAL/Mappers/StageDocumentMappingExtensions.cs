using discipline.centre.activityrules.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal static class StageDocumentMappingExtensions
{
    internal static Stage MapAsEntity(this StageDocument stageDocument)
        => new Stage(StageId.Parse(stageDocument.Id), stageDocument.Title, stageDocument.Index);
}