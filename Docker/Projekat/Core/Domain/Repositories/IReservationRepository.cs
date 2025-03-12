using System;
using Domain.Entities;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Domain.Repositories;

public interface IReservationRepository
{
    Task<PagedList<Reservation>> GetAllForUserIdAsync(Guid id, ReservationParameters requestParameters, CancellationToken cancellationToken = default);
    Task<Reservation> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<Reservation>> GetReservationsForArrangementIds(IEnumerable<Guid> arrangementIds, ReservationParameters reservationParameters, CancellationToken cancellationToken = default);
    Task InsertAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
}
