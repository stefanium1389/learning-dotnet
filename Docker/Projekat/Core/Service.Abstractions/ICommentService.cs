using System;
using Shared.Dtos;

namespace Service.Abstractions;

public interface ICommentService
{
    Task<CommentDto> ApproveCommentAsync(string id, CancellationToken cancellationToken =default);
    Task<CommentDto> DeclineCommentAsync(string id, CancellationToken cancellationToken =default);
    Task<CommentDto> CreateCommentAsync(CommentCreationDto dto, string userId, CancellationToken cancellationToken = default);
}
