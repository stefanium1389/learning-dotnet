using System;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Extensions;
using Shared.RequestFeatures;

namespace Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RepositoryDbContext _dbContext;

    public ReservationRepository(RepositoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Reservation>> GetAllForUserIdAsync(Guid id, ReservationParameters requestParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Reservations.Where(r => r.User.Id == id)
                                .Include(r => r.User)
                                .Include(r => r.AccomodationUnit)
                                .Include(r => r.Arrangement)
                                .ThenInclude(a => a.Accomodation)
                                .ThenInclude(a => a.AccomodationUnits)
                                .Filter(requestParameters)
                                .Sort(requestParameters.OrderBy!);

        var count = await query.CountAsync(cancellationToken);
        var list = await query.Paginate(requestParameters)
                                .ToListAsync(cancellationToken);
        return new PagedList<Reservation>(list, count, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations.Include(r => r.Arrangement).ThenInclude(a => a.Accomodation).ThenInclude(a => a.AccomodationUnits)
                                            .Include(r => r.AccomodationUnit)
                                            .Include(r => r.User)
                                            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<PagedList<Reservation>> GetReservationsForArrangementIds(IEnumerable<Guid> arrangementIds, ReservationParameters reservationParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Reservations.Where(r => arrangementIds.Contains(r.Arrangement.Id))
                                            .Include(r => r.Arrangement).ThenInclude(r => r.Accomodation).ThenInclude(a => a.AccomodationUnits)
                                            .Include(r => r.AccomodationUnit)
                                            .Include(r => r.User)
                                            .Filter(reservationParameters)
                                            .Sort(reservationParameters.OrderBy!);
        var count = await query.CountAsync(cancellationToken);
        var list = await query.Paginate(reservationParameters).ToListAsync(cancellationToken);
        return new PagedList<Reservation>(list, count, reservationParameters.PageNumber, reservationParameters.PageSize);
    }

    public async Task InsertAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _dbContext.Reservations.Update(reservation);
        if (_dbContext.Entry(reservation.AccomodationUnit).State == EntityState.Detached)
        {
            _dbContext.AccomodationUnits.Update(reservation.AccomodationUnit);
        }
        else
        {
            _dbContext.Entry(reservation.AccomodationUnit).State = EntityState.Modified;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
