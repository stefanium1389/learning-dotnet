using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Extensions;
using Shared.RequestFeatures;

namespace Persistence.Repositories;

public class ArrangementRepository : IArrangementRepository
{
    private readonly RepositoryDbContext _dbContext;
    public ArrangementRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagedList<Arrangement>> GetAllByCreatorAsync(ArrangementParameters arrangementParameters, User createdBy, CancellationToken cancellationToken = default)
    {
        User manager = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == createdBy.Id && u.Role == Role.Manager, cancellationToken) 
        ?? throw new ManagerNotFoundException(createdBy.Username);
        var query = manager.CreatedArrangements.AsQueryable()
                                            .Filter(arrangementParameters)
                                            .Search(arrangementParameters)
                                            .Sort(arrangementParameters.OrderBy!);
                                            
        var count = await query.CountAsync(cancellationToken);
        var arrangements = await query.Paginate(arrangementParameters).ToListAsync(cancellationToken);
        return PagedList<Arrangement>.ToPagedList(arrangements, count, arrangementParameters.PageNumber, arrangementParameters.PageSize);
    }

    public async Task<PagedList<Arrangement>> GetAllCurrentAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Arrangements.Include(a => a.Accomodation).ThenInclude(a => a.AccomodationUnits)
                                            .Where(a => a.StartDate > DateOnly.FromDateTime(DateTime.Now))
                                            .Filter(arrangementParameters)
                                            .Search(arrangementParameters)
                                            .Sort(arrangementParameters.OrderBy!);
        var count = await query.CountAsync(cancellationToken);
        var arrangements = await query.Paginate(arrangementParameters).ToListAsync(cancellationToken);
        return PagedList<Arrangement>.ToPagedList(arrangements, count, arrangementParameters.PageNumber, arrangementParameters.PageSize);
    }

    public async Task<PagedList<Arrangement>> GetAllPastAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Arrangements.Include(a => a.Accomodation).ThenInclude(a => a.AccomodationUnits)
                                            .Where(a => a.StartDate <= DateOnly.FromDateTime(DateTime.Now))
                                            .Filter(arrangementParameters)
                                            .Search(arrangementParameters)
                                            .Sort(arrangementParameters.OrderBy!);
        var count = await query.CountAsync(cancellationToken);
        var arrangements = await query.Paginate(arrangementParameters).ToListAsync(cancellationToken);
        return PagedList<Arrangement>.ToPagedList(arrangements, count, arrangementParameters.PageNumber, arrangementParameters.PageSize);
    }

    public async Task<Arrangement?> GetArrangementByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Arrangement? a = await _dbContext.Arrangements.Include(a => a.Accomodation)
                                            .ThenInclude(a => a.AccomodationUnits)
                                            .Include(a => a.MeetingPlace)
                                            .ThenInclude(mp => mp.Address)
                                            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return a;
    }

    public async Task InsertAsync(Arrangement arrangement, CancellationToken cancellationToken = default)
    {
        _dbContext.Arrangements.Add(arrangement);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Arrangement arrangement, CancellationToken cancellationToken = default)
    {
        _dbContext.Arrangements.Update(arrangement);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> IsValidForDeletion(Guid id, CancellationToken cancellationToken = default)
    {
        return !await _dbContext.Reservations.AnyAsync(r => r.Arrangement.Id == id, cancellationToken);
    }
}
