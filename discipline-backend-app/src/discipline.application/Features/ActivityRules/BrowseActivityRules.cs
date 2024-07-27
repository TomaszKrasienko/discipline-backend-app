using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace discipline.application.Features.ActivityRules;

internal static class BrowseActivityRules
{
    internal static WebApplication MapBrowseActivityRules(this WebApplication app)
    {
        app.MapGet($"/{Extensions.ActivityRulesTag}", async ([AsParameters]PaginationDto paginationDto, 
            HttpContext httpContext, IDisciplineMongoCollection disciplineMongoCollection) =>
            {
                var source = disciplineMongoCollection
                    .GetCollection<ActivityRuleDocument>().Find(_ => true);
                var pagedList = await PagedList<ActivityRuleDocument>
                    .ToPagedList(source, paginationDto.PageNumber, paginationDto.PageSize);
                httpContext.AddPaginationToHeader(pagedList);
                return Results.Ok(pagedList.Select(x => x.AsDto()));
                })
            .Produces(StatusCodes.Status200OK, typeof(List<ActivityRuleDto>))
            .WithName(nameof(BrowseActivityRules))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = $"Browses activity rules by pagination data. Adds pagination meta data in header with name {PagingBehaviour.HeaderName}"
            });
        return app;
    }
}