using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class DailyTracker : AggregateRoot<DailyTrackerId>
{
    public UserId UserId { get; private set; }
    
    public DailyTracker(DailyTrackerId id, UserId userId) : base(id)
    {
        UserId = userId;
    }
}