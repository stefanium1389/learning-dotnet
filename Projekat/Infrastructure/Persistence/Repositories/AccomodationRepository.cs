using System;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Extensions;
using Shared.RequestFeatures;

namespace Persistence.Repositories;

public class AccomodationRepository : IAccomodationRepository
{
    private readonly RepositoryDbContext _dbContext;
    public AccomodationRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagedList<Accomodation>> GetAllAsync(AccomodationParameters requestParameters, CancellationToken cancellationToken = default) 
    {
        var query = _dbContext.Accomodations.Search(requestParameters)
                                        .Sort(requestParameters.OrderBy!);
        var count = await query.CountAsync(cancellationToken);
        var users = await query
                                        .Paginate(requestParameters)
                                        .ToListAsync(cancellationToken);

        return PagedList<Accomodation>.ToPagedList(users, count, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<Accomodation?> GetAccomodationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Accomodations.Include(a => a.AccomodationUnits)
                                            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
    public async Task<PagedList<AccomodationUnit>> GetUnitsForAccomodationAsync(Guid id,AccomodationUnitParameters accomodationUnitParameters, CancellationToken cancellationToken = default)
    {
        Accomodation? a = await GetAccomodationByIdAsync(id, cancellationToken);
        var query = a!.AccomodationUnits.AsQueryable()
                    .Filter(accomodationUnitParameters)
                    .Sort(accomodationUnitParameters.OrderBy!);
        var count = query.Count();
        var list = query.Paginate(accomodationUnitParameters).ToList();

        return new PagedList<AccomodationUnit>(list, count, accomodationUnitParameters.PageNumber, accomodationUnitParameters.PageSize); 
    }

    public async Task InsertAsync(Accomodation accomodation, CancellationToken cancellationToken = default)
    {
        _dbContext.Accomodations.Add(accomodation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Accomodation accomodation, CancellationToken cancellationToken = default)
    {
        _dbContext.Accomodations.Update(accomodation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsValidForDeletionAsync(Guid id)
    {
        return !await _dbContext.Arrangements.AnyAsync(a=> a.StartDate > DateOnly.FromDateTime(DateTime.Now) && a.Accomodation.Id == id);
    }

    public async Task<AccomodationUnit?> GetUnitByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AccomodationUnits.FirstOrDefaultAsync(u => u.Id == guid);
    }

    public async Task UpdateUnitAsync(AccomodationUnit unit, CancellationToken cancellationToken = default)
    {
        _dbContext.AccomodationUnits.Update(unit);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task InsertUnitAsync(AccomodationUnit unit, CancellationToken cancellationToken = default)
    {
        _dbContext.AccomodationUnits.Add(unit);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsUnitVaildForEditingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return !await _dbContext.Reservations
                    .AnyAsync(r => r.Arrangement.StartDate > DateOnly.FromDateTime(DateTime.Now) 
                                && r.Arrangement.Accomodation.AccomodationUnits.Any(u => u.Id == id), cancellationToken);
    }

    public async Task<bool> IsUnitBookedAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations.AnyAsync(r => r.AccomodationUnit.Id == guid && r.IsActive == false, cancellationToken);
    }
}
