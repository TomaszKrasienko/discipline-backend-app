using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
using discipline.centre.dailytrackers.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

/// <summary>
/// Extensions that map Daily Tracker Document on related types
/// </summary>
internal static class DailyTrackerDocumentMappingExtensions
{
    /// <summary>
    /// Maps <see cref="DailyTrackerDocument"/> on <see cref="DailyTracker"/>
    /// </summary>
    /// <param name="document">Instance of <see cref="DailyTrackerDocument"/> to be mapped</param>
    /// <returns>Mapped instance of <see cref="DailyTracker"/></returns>
    internal static DailyTracker AsEntity(this DailyTrackerDocument document)
        => new (
            DailyTrackerId.Parse(document.DailyTrackerId),
            document.Day,
            UserId.Parse(document.UserId),
            document.Activities.Select(x => x.AsEntity()).ToList());

    /// <summary>
    /// Maps <see cref="DailyTrackerDocument"/> as <see cref="DailyTrackerDto"/>
    /// </summary>
    /// <param name="document">Instance of <see cref="DailyTrackerDocument"/> to be mapped</param>
    /// <returns>Mapped instance of <see cref="DailyTrackerDto"/></returns>
    internal static DailyTrackerDto AsDto(this DailyTrackerDocument document)
        => new(document.DailyTrackerId, document.Day, document.Activities.Select(x => x.AsDto()).ToArray());
}