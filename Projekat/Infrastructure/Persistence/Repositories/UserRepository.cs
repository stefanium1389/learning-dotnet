using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using Persistence.Repositories.Extensions;
using Shared.RequestFeatures;


namespace Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RepositoryDbContext _dbContext;
    public UserRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagedList<User>> GetAllAsync(UserParameters userParameters, CancellationToken cancellationToken = default) 
    {
        var usersQuery = _dbContext.Users.Search(userParameters)
                                        .Sort(userParameters.OrderBy!);
        var count = await usersQuery.CountAsync(cancellationToken);
        var users = await usersQuery
                                        .Paginate(userParameters)
                                        .ToListAsync(cancellationToken);

        return PagedList<User>.ToPagedList(users, count, userParameters.PageNumber, userParameters.PageSize);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.Include(u => u.CreatedArrangements)
                                    .Include(u => u.Reservations)
                                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.Include(u => u.CreatedArrangements)
                                    .Include(u => u.Reservations)
                                    .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<PagedList<User>> GetSuspiciousUsersAsync(UserParameters userParameters, CancellationToken cancellationToken)
    {
        var usersQuery = _dbContext.Users.Where(u => u.Reservations.Count(r=>!r.IsActive) >= 2)
                                        .Search(userParameters)
                                        .Sort(userParameters.OrderBy!);
        var count = await usersQuery.CountAsync(cancellationToken);
        var users = await usersQuery
                                        .Paginate(userParameters)
                                        .ToListAsync(cancellationToken);

        return PagedList<User>.ToPagedList(users, count, userParameters.PageNumber, userParameters.PageSize);
    }

    public async Task InsertAsync(User user, CancellationToken cancellationToken = default)
    {
         _dbContext.Users.Add(user);
         await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

}
