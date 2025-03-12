using System;
using Domain.Entities;
using Shared.RequestFeatures;

namespace Domain.Repositories;

public interface IAccomodationRepository
{
    Task<Accomodation> GetAccomodationByIdAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<PagedList<Accomodation>> GetAllAsync(AccomodationParameters requestParameters, CancellationToken cancellationToken = default);
    Task<AccomodationUnit> GetUnitByIdAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<PagedList<AccomodationUnit>> GetUnitsForAccomodationAsync(Guid id, AccomodationUnitParameters accomodationUnitParameters, CancellationToken cancellationToken = default);
    Task InsertAsync(Accomodation accomodation, CancellationToken cancellationToken = default);
    Task<bool> IsValidForDeletionAsync(Guid id);
    Task UpdateAsync(Accomodation accomodation, CancellationToken cancellationToken = default);
    Task UpdateUnitAsync(AccomodationUnit unit, CancellationToken cancellationToken = default);
    Task InsertUnitAsync(AccomodationUnit unit, CancellationToken cancellationToken = default);
    Task<bool> IsUnitVaildForEditingAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsUnitBookedAsync(Guid guid, CancellationToken cancellationToken = default);
}
