using System;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Extensions;
using Shared.RequestFeatures;

namespace Persistence.Repositories;

public class CommentRepository : ICommentRepository
{

    private readonly RepositoryDbContext _dbContext;
    public CommentRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagedList<Comment>> GetApprovedCommentsForArrangementAsync(Guid arrangementId, CommentParameters commentParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Comments.Where(c => c.Arrangement.Id == arrangementId && c.Status == CommentStatus.Approved)
                                        .Include(c=> c.Arrangement)
                                        .ThenInclude(a=> a.Accomodation)
                                        .ThenInclude(a=>a.AccomodationUnits)
                                        .Include(c => c.User);
        var count = await query.CountAsync(cancellationToken);
        var list = await query.Paginate(commentParameters).ToListAsync(cancellationToken);
        return new PagedList<Comment>(list, count, commentParameters.PageNumber, commentParameters.PageSize);
    }
    public async Task<PagedList<Comment>> GetAllCommentsForArrangementAsync(Guid arrangementId, CommentParameters commentParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Comments.Where(c => c.Arrangement.Id == arrangementId)
                                        .Include(c=> c.Arrangement)
                                        .ThenInclude(a=> a.Accomodation)
                                        .ThenInclude(a=>a.AccomodationUnits)
                                        .Include(c => c.User);
        var count = await query.CountAsync(cancellationToken);
        var list = await query.Paginate(commentParameters).ToListAsync(cancellationToken);
        return new PagedList<Comment>(list, count, commentParameters.PageNumber, commentParameters.PageSize);
    }
    public async Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Comments.Include(c=> c.Arrangement)
                                        .ThenInclude(a=> a.Accomodation)
                                        .ThenInclude(a=>a.AccomodationUnits)
                                        .Include(c => c.User)
                                        .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task InsertAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _dbContext.Comments.Add(comment);
         await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _dbContext.Comments.Remove(comment);
         await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _dbContext.Comments.Update(comment);
         await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
