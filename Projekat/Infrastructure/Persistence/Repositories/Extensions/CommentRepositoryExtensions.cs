using System;
using Domain.Entities;
using Persistence.Repositories.Extensions.Utility;
using Shared.RequestFeatures;

namespace Persistence.Repositories.Extensions;

public static class CommentRepositoryExtensions
{
    public static IQueryable<Comment> Paginate(this IQueryable<Comment> comments, RequestParameters requestParameters)
    {
        return Paginator.Paginate(comments, requestParameters);
    }
}
