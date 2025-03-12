using System;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service.Abstractions;

public interface IAccomodationService
{
    Task<AccomodationDto> GetAccomodationByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<AccomodationUnitDto>, MetaData)> GetAccomodationUnitsForIdAsync(string id, AccomodationUnitParameters accomodationUnitParameters, CancellationToken cancellationToken = default);
    Task<(IEnumerable<AccomodationDto>, MetaData)> GetAccomodationsAsync(AccomodationParameters accomodationParameters, CancellationToken cancellationToken = default);
    Task<AccomodationDto> CreateAccomodationAsync(AccomodationCreationDto dto, CancellationToken cancellationToken = default);
    Task<AccomodationDto> UpdateAccomodationAsync(AccomodationCreationDto dto, string id, CancellationToken cancellationToken = default);
    Task DeleteAccomodationAsync(string id, CancellationToken cancellationToken = default);
    Task<AccomodationUnitDto> CreateAccomodationUnitAsync(string id, AccomodationUnitCreationDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAccomodationUnitAsync(string id, CancellationToken cancellationToken = default);
    Task<AccomodationUnitDto> UpdateAccomodationUnitAsync(string unitId, AccomodationUnitCreationDTO dto, CancellationToken cancellationToken = default);
}
