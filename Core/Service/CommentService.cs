using System;
using System.Xml;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.Dtos;

namespace Service;

public class CommentService : ICommentService
{
    private readonly IRepositoryManager _repositoryManager;

    public CommentService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<CommentDto> ApproveCommentAsync(string id, CancellationToken cancellationToken = default)
    {
        Comment comment = await _repositoryManager.CommentRepository.GetCommentByIdAsync(Guid.Parse(id), cancellationToken);
        comment.Status = CommentStatus.Approved;
        await _repositoryManager.CommentRepository.UpdateAsync(comment, cancellationToken);
        return comment.Adapt<CommentDto>();
    }

    public async Task<CommentDto> CreateCommentAsync(CommentCreationDto dto, string userId, CancellationToken cancellationToken = default)
    {
        Arrangement arrangement = await _repositoryManager.ArrangementRepository.GetArrangementByIdAsync(Guid.Parse(dto.ArrangementId), cancellationToken);
        if(arrangement.EndDate >= DateOnly.FromDateTime(DateTime.Now))
        {
            throw new CommentException("You can only make comments on past arrangements!");
        }
        User user = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
        if(!user.Reservations.Any(r=> r.Arrangement.Id == arrangement.Id))
        {
            throw new CommentException("You can only make comments on arrangements you were on!");
        }
        Comment comment = new()
        {
            Arrangement = arrangement,
            User = user,
            Rating = dto.Rating,
            CommentText = dto.CommentText,
            Status = CommentStatus.Pending
        };
        await _repositoryManager.CommentRepository.InsertAsync(comment, cancellationToken);
        return comment.Adapt<CommentDto>();
    }

    public async Task<CommentDto> DeclineCommentAsync(string id, CancellationToken cancellationToken = default)
    {
        Comment comment = await _repositoryManager.CommentRepository.GetCommentByIdAsync(Guid.Parse(id), cancellationToken);
        comment.Status = CommentStatus.Declined;
        await _repositoryManager.CommentRepository.UpdateAsync(comment);
        return comment.Adapt<CommentDto>();
    }
}
