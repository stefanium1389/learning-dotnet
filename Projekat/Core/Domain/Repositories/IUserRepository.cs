using System;
using Domain.Entities;
using Shared.RequestFeatures;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllAsync(UserParameters userParameters, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<PagedList<User>> GetSuspiciousUsersAsync(UserParameters userParameters, CancellationToken cancellationToken);
    Task InsertAsync(User user, CancellationToken cancellationToken = default);
    Task RemoveAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}
