using System;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service;

public class ReservationService : IReservationService
{
    private readonly IRepositoryManager _repositoryManager;

    public ReservationService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ReservationDto> CancelReservationAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        Reservation reservation = await _repositoryManager.ReservationRepository.GetByIdAsync(Guid.Parse(id), cancellationToken);
        if(reservation.User.Id != Guid.Parse(userId))
        {
            throw new ReservationFailedException("You cannot edit someone elses reservaton!");
        }
        if(reservation.Arrangement.StartDate < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ReservationFailedException("You cannot cancel reservations in the past!");
        }
        reservation.IsActive = false;
        reservation.AccomodationUnit.IsBooked = false;
        await _repositoryManager.ReservationRepository.UpdateAsync(reservation, cancellationToken);
        await _repositoryManager.AccomodationRepository.UpdateUnitAsync(reservation.AccomodationUnit, cancellationToken);
        return reservation.Adapt<ReservationDto>();
    }

    public async Task<ReservationDto> CreateReservationAsync(ReservationCreationDto dto, string userId, CancellationToken cancellationToken = default)
    {
        User tourist = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
        Arrangement arrangement = await _repositoryManager.ArrangementRepository.GetArrangementByIdAsync(Guid.Parse(dto.ArrangementId),cancellationToken);
        if(arrangement.StartDate < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ReservationFailedException("You cannot make a reservation in the past!");
        }
        AccomodationUnit unit = await _repositoryManager.AccomodationRepository.GetUnitByIdAsync(Guid.Parse(dto.AccomodationUnitId),cancellationToken);
        if(unit.IsBooked)
        {
            throw new ReservationFailedException("Accomodation Unit is already booked!");
        }
        Reservation reservation = new()
        {
            User = tourist,
            Arrangement = arrangement,
            AccomodationUnit = unit,
            IsActive = true
        };
        unit.IsBooked = true;
        await _repositoryManager.AccomodationRepository.UpdateUnitAsync(unit, cancellationToken);
        await _repositoryManager.ReservationRepository.InsertAsync(reservation, cancellationToken);
        return reservation.Adapt<ReservationDto>();
    }

    

    public async Task<ReservationDto> GetReservationById(string id, CancellationToken cancellationToken = default)
    {
        Reservation reservation = await _repositoryManager.ReservationRepository.GetByIdAsync(Guid.Parse(id), cancellationToken);
        return reservation.Adapt<ReservationDto>();
    }

    public async Task<(IEnumerable<ReservationDto>, MetaData)> GetReservationsForManagersArrangementsByUserId(string id, ReservationParameters reservationParameters, CancellationToken cancellationToken = default)
    {
        User manager = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(id), cancellationToken);
        List<Guid> arrangementIds = [.. manager.CreatedArrangements.Select(a => a.Id)];
        var reservationsWithMetaData = await _repositoryManager.ReservationRepository.GetReservationsForArrangementIds(arrangementIds, reservationParameters, cancellationToken);
        var dtos = reservationsWithMetaData.Adapt<IEnumerable<ReservationDto>>();
        return (dtos, reservationsWithMetaData.MetaData);

    }

    public async Task<(IEnumerable<ReservationDto>, MetaData)> GetReservationsForUserId(string userId, ReservationParameters requestParameters, CancellationToken cancellationToken = default)
    {
        var reservationsWithMetaData = await _repositoryManager.ReservationRepository.GetAllForUserIdAsync(Guid.Parse(userId), requestParameters, cancellationToken);
        var reservationsDto = reservationsWithMetaData.Adapt<IEnumerable<ReservationDto>>();
        return (reservationsDto, reservationsWithMetaData.MetaData);
    }

}
