using discipline.application.DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace discipline.application.Behaviours;

internal static class PagingBehaviour
{
    internal const string HeaderName = "x-pagination";

    internal static void AddPaginationToHeader<T>(this HttpContext context, PagedList<T> pagedList)
    {
        var metaDataDto = pagedList.AsMetaData();
        context.Response.Headers.TryAdd(HeaderName, JsonConvert.SerializeObject(metaDataDto));
    }

    private static MetaDataDto AsMetaData<T>(this PagedList<T> pagedList)
        => new MetaDataDto()
        {
            CurrentPage = pagedList.CurrentPage,
            TotalPages = pagedList.TotalPages,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            HasPrevious = pagedList.HasPrevious,
            HasNext = pagedList.HasNext
        };
}

internal class PagedList<T> : List<T>
{
    internal int CurrentPage { get; }
    internal int TotalPages { get; }
    internal int PageSize { get; }
    internal long TotalCount { get; }
    internal bool HasPrevious => CurrentPage > 1;
    internal bool HasNext => CurrentPage < TotalPages;

    private PagedList(IEnumerable<T> items, long count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    // internal static async Task<PagedList<T>> ToPagedList(IFindFluent<T,T> source, int pageNumber, int pageSize)
    // {
    //     var count = await source.CountDocumentsAsync();
    //     var items = await source
    //         .Skip((pageNumber - 1) * pageSize)
    //         .Limit(pageSize)
    //         .ToListAsync();
    //     return new PagedList<T>(items, count, pageNumber, pageSize);
    // }
}