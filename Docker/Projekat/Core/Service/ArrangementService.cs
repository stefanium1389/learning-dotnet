using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service;

public class ArrangementService : IArrangementService
{
    private readonly IRepositoryManager _repositoryManager;

    public ArrangementService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ArrangementDto> GetArrangementByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        Arrangement arrangement = await _repositoryManager.ArrangementRepository.GetArrangementByIdAsync(Guid.Parse(id), cancellationToken);
        var arrangementDto = arrangement.Adapt<ArrangementDto>();
        return arrangementDto;
    }

    public async Task<(IEnumerable<CommentDto>, MetaData)> GetApprovedArrangementCommentsAsync(CommentParameters commentParameters, string id, CancellationToken cancellationToken = default)
    {
        var commentsWithMetaData = await _repositoryManager.CommentRepository.GetApprovedCommentsForArrangementAsync(Guid.Parse(id), commentParameters, cancellationToken);
        var commentDtos = commentsWithMetaData.Adapt<IEnumerable<CommentDto>>();
        return (commentDtos, commentsWithMetaData.MetaData);
    }
    public async Task<(IEnumerable<CommentDto>, MetaData)> GetAllArrangementCommentsAsync(CommentParameters commentParameters, string id, string userId, CancellationToken cancellationToken = default)
    {
        User manager = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(userId));
        if(!manager.CreatedArrangements.Any(a => a.Id == Guid.Parse(id)))
        {
            throw new CommentException("Manager can only view comments on his own arrangements!");
        }
        var commentsWithMetaData = await _repositoryManager.CommentRepository.GetAllCommentsForArrangementAsync(Guid.Parse(id), commentParameters, cancellationToken);
        var commentDtos = commentsWithMetaData.Adapt<IEnumerable<CommentDto>>();
        return (commentDtos, commentsWithMetaData.MetaData);
    }

    public async Task<(IEnumerable<ArrangementPreviewDto>, MetaData)> GetPastAndCurrentArrangementsAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default)
    {
        var arrangementsWithMetaData = await _repositoryManager.ArrangementRepository.GetAllPastAsync(arrangementParameters, cancellationToken);
        var arrangementDtos = arrangementsWithMetaData.Adapt<IEnumerable<ArrangementPreviewDto>>();
        return (arrangementDtos, arrangementsWithMetaData.MetaData);
    }

    public async Task<(IEnumerable<ArrangementPreviewDto>, MetaData)> GetUpcomingArrangementsAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default)
    {
        var arrangementsWithMetaData = await _repositoryManager.ArrangementRepository.GetAllCurrentAsync(arrangementParameters, cancellationToken);
        var arrangementDtos = arrangementsWithMetaData.Adapt<IEnumerable<ArrangementPreviewDto>>();
        return (arrangementDtos, arrangementsWithMetaData.MetaData);
    }

    public async Task<ArrangementDto> CreateArrangementAsync(ArrangementCreationDto dto, string userId,CancellationToken cancellationToken = default)
    {
        User manager = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(userId));
        Address address = dto.MeetingPlace.Address.Adapt<Address>();
        MeetingPlace meetingPlace = dto.MeetingPlace.Adapt<MeetingPlace>();
        meetingPlace.Address = address;
        Arrangement a = dto.Adapt<Arrangement>();
        a.MeetingPlace = meetingPlace;
        a.Accomodation = await _repositoryManager.AccomodationRepository.GetAccomodationByIdAsync(Guid.Parse(dto.AccomodationId), cancellationToken);
        manager.CreatedArrangements.Add(a);
        await _repositoryManager.ArrangementRepository.InsertAsync(a, cancellationToken);
        await _repositoryManager.UserRepository.UpdateAsync(manager, cancellationToken);
        return a.Adapt<ArrangementDto>();
    }

    public async Task DeleteArrangementAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        if(await IsOwnArrangement(id, userId, cancellationToken))
        {
            throw new NotOwnArrangementException();
        }
        if(!await _repositoryManager.ArrangementRepository.IsValidForDeletion(Guid.Parse(id), cancellationToken))
        {
            throw new ArrangementHasFutureReservationsException();
        }
        Arrangement arrangement = await _repositoryManager.ArrangementRepository.GetArrangementByIdAsync(Guid.Parse(id), cancellationToken);
        arrangement.IsDeleted = true;
        await _repositoryManager.ArrangementRepository.UpdateAsync(arrangement, cancellationToken);
    }

    public async Task<ArrangementDto> UpdateArrangementAsync(ArrangementCreationDto dto, string id, string userId, CancellationToken cancellationToken = default)
    {
        if(await IsOwnArrangement(id, userId, cancellationToken))
        {
            throw new NotOwnArrangementException();
        }
        Arrangement arrangement = await _repositoryManager.ArrangementRepository.GetArrangementByIdAsync(Guid.Parse(id), cancellationToken);
        if(dto.ArrangementType is not null)
        {
            try
            {
                arrangement.ArrangementType = Enum.Parse<ArrangementType>(dto.ArrangementType);
            }
            catch
            {
                throw new InvalidEnumValueExcepton(dto.ArrangementType, "ArrangementType");
            }
        }
        if(dto.TransportationType is not null)
        {
            try
            {
                arrangement.TransportationType = Enum.Parse<TransportationType>(dto.TransportationType);
            }
            catch
            {
                throw new InvalidEnumValueExcepton(dto.TransportationType, "TransportationType");
            }
        }
        arrangement.Name = dto.Name ?? arrangement.Name;
        arrangement.Destination = dto.Destination ?? arrangement.Destination;
        arrangement.Description = dto.Description ?? arrangement.Description;
        arrangement.TravelProgramme = dto.Destination ?? arrangement.TravelProgramme;
        arrangement.PosterUrl = dto.PosterUrl ?? arrangement.PosterUrl;
        if(dto.MeetingPlace is not null)
        {
            if(dto.MeetingPlace.Address is not null)
            {
                arrangement.MeetingPlace.Address.Street = dto.MeetingPlace.Address.Street ?? arrangement.MeetingPlace.Address.Street;
                arrangement.MeetingPlace.Address.Number = dto.MeetingPlace.Address.Number ?? arrangement.MeetingPlace.Address.Number;
                arrangement.MeetingPlace.Address.City = dto.MeetingPlace.Address.City ?? arrangement.MeetingPlace.Address.City;
                arrangement.MeetingPlace.Address.PostalCode = dto.MeetingPlace.Address.PostalCode ?? arrangement.MeetingPlace.Address.PostalCode;
            }
            arrangement.MeetingPlace.Latitude = dto.MeetingPlace.Latitude ?? arrangement.MeetingPlace.Latitude;
            arrangement.MeetingPlace.Longitude = dto.MeetingPlace.Longitude ?? arrangement.MeetingPlace.Longitude;
        }
        if(dto.StartDate != DateOnly.FromDayNumber(0))
        {
            arrangement.StartDate = dto.StartDate;
        }
        if(dto.EndDate != DateOnly.FromDayNumber(0))
        {
            arrangement.EndDate = dto.EndDate;
        }
        if(dto.MeetingTime.HasValue)
        {
            arrangement.MeetingTime = dto.MeetingTime.Value;
        }
        if(dto.AccomodationId is not null)
        {
            arrangement.Accomodation = await _repositoryManager.AccomodationRepository.GetAccomodationByIdAsync(Guid.Parse(dto.AccomodationId), cancellationToken);
        }
        await _repositoryManager.ArrangementRepository.UpdateAsync(arrangement, cancellationToken);
        return arrangement.Adapt<ArrangementDto>();
    }
    
    public async Task<(IEnumerable<ArrangementPreviewDto>, MetaData)> GetArrangementsForManager(ArrangementParameters arrangementParameters, string userId, CancellationToken cancellationToken = default)
    {
        User manager = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
        var arrangementsWithMetaData = await _repositoryManager.ArrangementRepository.GetAllByCreatorAsync(arrangementParameters, manager, cancellationToken);
        var arrangementDtos = arrangementsWithMetaData.Adapt<IEnumerable<ArrangementPreviewDto>>();
        return (arrangementDtos, arrangementsWithMetaData.MetaData);
    }

    private async Task<bool> IsOwnArrangement(string arrangementId, string userId, CancellationToken cancellationToken = default)
    {
        User manager = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
        Arrangement? a = manager.CreatedArrangements.FirstOrDefault(a => a.Id == Guid.Parse(arrangementId));
        if (a is null)
        {
            return false;
        }
        return true;
    }
}
