using System;
using Shared.RequestFeatures;

namespace Persistence.Repositories.Extensions.Utility;

public static class Paginator
{
    public static IQueryable<T> Paginate<T>(IQueryable<T> queryable, RequestParameters requestParameters )
    {
        return queryable.Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
                        .Take(requestParameters.PageSize);
    }
}
