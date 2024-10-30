using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.Behaviours.IdentityContext;
using discipline.application.DTOs;
using discipline.application.Features.ActivityRules.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

internal static class BrowseActivityRules
{
    internal static WebApplication MapBrowseActivityRules(this WebApplication app)
    {
        app.MapGet($"/{Extensions.ActivityRulesTag}", async ([AsParameters] PaginationDto paginationDto,
                HttpContext httpContext,
                IIdentityContext identityContext) =>
            {
                // var source = disciplineMongoCollection
                //     .GetCollection<ActivityRuleDocument>()
                //     .Find(x => x.UserId == identityContext.UserId.ToString());
                // var pagedList = await PagedList<ActivityRuleDocument>
                //     .ToPagedList(source, paginationDto.PageNumber, paginationDto.PageSize);
                // httpContext.AddPaginationToHeader(pagedList);
                // return Results.Ok(pagedList.Select(x => x.AsDto()));
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status200OK, typeof(List<ActivityRuleDto>))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName(nameof(BrowseActivityRules))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new(operation)
            {
                Description =
                    $"Browses activity rules by pagination data. Adds pagination meta data in header with name {PagingBehaviour.HeaderName}"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}