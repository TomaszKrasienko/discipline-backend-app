using discipline.application.Behaviours;
using discipline.application.Domain.Entities;
using discipline.application.DTOs;
using discipline.application.DTOs.Mappers;
using discipline.application.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Features.ActivityRules;

internal static class BrowseActivityRules
{
    internal static WebApplication MapBrowseActivityRules(this WebApplication app)
    {
        app.MapGet("/activity-rules", async ([AsParameters]PaginationDto paginationDto, 
            HttpContext httpContext, DisciplineDbContext dbContext) =>
        {
            var source = dbContext
                .ActivityRules
                .AsNoTracking();
            var pagedList = await PagedList<ActivityRule>.ToPagedList(source,
                paginationDto.PageNumber, paginationDto.PageSize);
            httpContext.AddPaginationToHeader(pagedList);
            return Results.Ok(pagedList.Select(x => x.AsDto()));
        });
        return app;
    }
}