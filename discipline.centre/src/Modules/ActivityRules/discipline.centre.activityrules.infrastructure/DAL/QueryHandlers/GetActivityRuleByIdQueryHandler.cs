using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.CQRS.Queries;
using MongoDB.Driver;

namespace discipline.centre.activityrules.infrastructure.DAL.QueryHandlers;

internal sealed class GetActivityRuleByIdQueryHandler(
    ActivityRulesMongoContext context) : IQueryHandler<GetActivityRuleByIdQuery, ActivityRuleDto?>
{
    public async Task<ActivityRuleDto?> HandleAsync(GetActivityRuleByIdQuery query, CancellationToken cancellationToken = default)
        => (await context.GetCollection<ActivityRuleDocument>()
            .Find(x 
                => x.Id == query.ActivityRuleIdId.ToString()
                && x.UserId == query.UserId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.MapAsDto();
}