using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Events;

public sealed record ActivityRuleDeleted(Ulid UserId, Ulid ActivityRuleId) : IEvent;