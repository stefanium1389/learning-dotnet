using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service;

public class AccomodationService : IAccomodationService
{
    private readonly IRepositoryManager _repositoryManager;

    public AccomodationService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    
    public async Task<AccomodationDto> CreateAccomodationAsync(AccomodationCreationDto dto, CancellationToken cancellationToken = default)
    {
        Accomodation accomodation = dto.Adapt<Accomodation>();
        await _repositoryManager.AccomodationRepository.InsertAsync(accomodation, cancellationToken);
        return accomodation.Adapt<AccomodationDto>();
    }

    public async Task<AccomodationUnitDto> CreateAccomodationUnitAsync(string id, AccomodationUnitCreationDTO dto, CancellationToken cancellationToken = default)
    {
        Accomodation a = await _repositoryManager.AccomodationRepository.GetAccomodationByIdAsync(Guid.Parse(id), cancellationToken);
        AccomodationUnit unit = dto.Adapt<AccomodationUnit>();
        a.AccomodationUnits.Add(unit);
        await _repositoryManager.AccomodationRepository.UpdateAsync(a);
        return unit.Adapt<AccomodationUnitDto>();
    }

    public async Task DeleteAccomodationAsync(string id, CancellationToken cancellationToken = default)
    {
        if(! await AccomodationValidForDeletion(id))
        {
            throw new AccomodationNotValidForDeletionException(id);
        }
        Accomodation accomodation = await _repositoryManager.AccomodationRepository.GetAccomodationByIdAsync(Guid.Parse(id), cancellationToken);
        accomodation.IsDeleted = true;
        await _repositoryManager.AccomodationRepository.UpdateAsync(accomodation, cancellationToken);
    }

    public async Task DeleteAccomodationUnitAsync(string id, CancellationToken cancellationToken = default)
    {
        if(! await UnitVaildForEditing(id))
        {
            throw new AccomodationUnitNotValidForDeletionException(id);
        }
        AccomodationUnit unit = await _repositoryManager.AccomodationRepository.GetUnitByIdAsync(Guid.Parse(id), cancellationToken);
        unit.IsDeleted = true;
        await _repositoryManager.AccomodationRepository.UpdateUnitAsync(unit, cancellationToken);
    }

    public async Task<AccomodationDto> GetAccomodationByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        Accomodation a = await _repositoryManager.AccomodationRepository.GetAccomodationByIdAsync(Guid.Parse(id), cancellationToken);
        return a.Adapt<AccomodationDto>();
        
    }

    public async Task<(IEnumerable<AccomodationDto>, MetaData)> GetAccomodationsAsync(AccomodationParameters accomodationParameters, CancellationToken cancellationToken = default)
    {
        var accomodationsWithMetaData = await _repositoryManager.AccomodationRepository.GetAllAsync(accomodationParameters, cancellationToken);
        var accomodationDtos = accomodationsWithMetaData.Adapt<IEnumerable<AccomodationDto>>();
        return (accomodationDtos, accomodationsWithMetaData.MetaData);
    }

    public async Task<(IEnumerable<AccomodationUnitDto>, MetaData)> GetAccomodationUnitsForIdAsync(string id, AccomodationUnitParameters accomodationUnitParameters, CancellationToken cancellationToken = default)
    {
        var unitsWithMetaData = await _repositoryManager.AccomodationRepository.GetUnitsForAccomodationAsync(Guid.Parse(id), accomodationUnitParameters, cancellationToken);
        var unitDtos = unitsWithMetaData.Adapt<IEnumerable<AccomodationUnitDto>>();
        return (unitDtos, unitsWithMetaData.MetaData);
    }

    public async Task<AccomodationDto> UpdateAccomodationAsync(AccomodationCreationDto dto, string id, CancellationToken cancellationToken = default)
    {
        Accomodation accomodation = await _repositoryManager.AccomodationRepository.GetAccomodationByIdAsync(Guid.Parse(id), cancellationToken);
        accomodation.Name = dto.Name ?? accomodation.Name;
        if(dto.AccomodationType is not null)
        {
            try
            {
                AccomodationType type = Enum.Parse<AccomodationType>(dto.AccomodationType);
                accomodation.AccomodationType = type;
            }
            catch
            {
                throw new InvalidEnumValueExcepton(dto.AccomodationType, "AccomodationType");
            }
        }
        accomodation.HasPool = dto.HasPool ?? accomodation.HasPool;
        accomodation.HasSpa = dto.HasSpa ?? accomodation.HasSpa;
        accomodation.HasWifi = dto.HasWifi ?? accomodation.HasWifi;
        accomodation.DisabledFriendly = dto.DisabledFriendly ?? accomodation.DisabledFriendly;
        accomodation.Stars = dto.Stars ?? accomodation.Stars;
        await _repositoryManager.AccomodationRepository.UpdateAsync(accomodation, cancellationToken);
        return accomodation.Adapt<AccomodationDto>();
    }

    public async Task<AccomodationUnitDto> UpdateAccomodationUnitAsync(string unitId, AccomodationUnitCreationDTO dto, CancellationToken cancellationToken = default)
    {
        AccomodationUnit unit = await _repositoryManager.AccomodationRepository.GetUnitByIdAsync(Guid.Parse(unitId));
        unit.Price = dto.Price ?? unit.Price;
        unit.PetsAllowed = dto.PetsAllowed ?? unit.PetsAllowed;
        if(dto.MaximumGuests is not null)
        {
            if(await UnitVaildForEditing(unitId))
            {
                unit.MaximumGuests = dto.MaximumGuests ?? unit.MaximumGuests;
            }
        }
        await _repositoryManager.AccomodationRepository.UpdateUnitAsync(unit);
        return unit.Adapt<AccomodationUnitDto>();
    }

    private async Task<bool> AccomodationValidForDeletion(string id)
    {
        return await _repositoryManager.AccomodationRepository.IsValidForDeletionAsync(Guid.Parse(id));
    }
    private async Task<bool> UnitVaildForEditing(string id)
    {
        return await _repositoryManager.AccomodationRepository.IsUnitVaildForEditingAsync(Guid.Parse(id));
    }
}
