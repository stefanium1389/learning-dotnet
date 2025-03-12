using System;
using Domain.Entities;
using Shared.RequestFeatures;

namespace Domain.Repositories;

public interface ICommentRepository
{
    Task<PagedList<Comment>> GetApprovedCommentsForArrangementAsync(Guid arrangementId, CommentParameters commentParameters, CancellationToken cancellationToken = default);
    Task<PagedList<Comment>> GetAllCommentsForArrangementAsync(Guid arrangementId, CommentParameters commentParameters, CancellationToken cancellationToken = default);
    Task<Comment> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(Comment comment, CancellationToken cancellationToken = default);
    Task RemoveAsync(Comment comment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
}
