using System;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service.Abstractions;

public interface IArrangementService
{
    Task<(IEnumerable<ArrangementPreviewDto>, MetaData)> GetUpcomingArrangementsAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default);
    Task<(IEnumerable<ArrangementPreviewDto>, MetaData)> GetPastAndCurrentArrangementsAsync(ArrangementParameters arrangementParameters, CancellationToken cancellationToken = default);
    Task<ArrangementDto> GetArrangementByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<CommentDto>, MetaData)> GetApprovedArrangementCommentsAsync(CommentParameters commentParameters, string id, CancellationToken cancellationToken = default);
    Task<ArrangementDto> CreateArrangementAsync(ArrangementCreationDto dto, string userId, CancellationToken cancellationToken = default);
    Task<ArrangementDto> UpdateArrangementAsync(ArrangementCreationDto dto, string id, string userId, CancellationToken cancellationToken = default);
    Task DeleteArrangementAsync(string id, string userId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<ArrangementPreviewDto>, MetaData)> GetArrangementsForManager(ArrangementParameters arrangementParameters, string userId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<CommentDto>, MetaData)> GetAllArrangementCommentsAsync(CommentParameters commentParameters, string id, string userId, CancellationToken cancellationToken = default);
}
