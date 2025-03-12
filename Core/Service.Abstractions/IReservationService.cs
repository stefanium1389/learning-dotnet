using System;
using System.Reflection.Metadata.Ecma335;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service.Abstractions;

public interface IReservationService
{
    Task<(IEnumerable<ReservationDto>, MetaData)> GetReservationsForUserId(string userId, ReservationParameters requestParameters, CancellationToken cancellationToken = default);
    Task<ReservationDto> CreateReservationAsync(ReservationCreationDto dto, string userId, CancellationToken cancellationToken = default);
    Task<ReservationDto> CancelReservationAsync(string id, string userId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<ReservationDto>, MetaData)> GetReservationsForManagersArrangementsByUserId(string id, ReservationParameters reservationParameters, CancellationToken cancellationToken = default);
    Task<ReservationDto> GetReservationById(string id, CancellationToken cancellationToken = default);
}
