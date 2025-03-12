using System;
using Domain.Entities;
using Shared.RequestFeatures;

namespace Domain.Repositories;

public interface IArrangementRepository
{
    Task<PagedList<Arrangement>> GetAllPastAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default);
    Task<PagedList<Arrangement>> GetAllCurrentAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default);
    Task<PagedList<Arrangement>> GetAllByCreatorAsync(ArrangementParameters arrangementParameters, User createdBy, CancellationToken cancellationToken = default);
    Task<Arrangement> GetArrangementByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(Arrangement arrangement, CancellationToken cancellationToken = default);
    Task UpdateAsync(Arrangement arrangement, CancellationToken cancellationToken = default);
    Task<bool> IsValidForDeletion(Guid id, CancellationToken cancellationToken = default);
}
