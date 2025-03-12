using Shared.Dtos;
using Shared.RequestFeatures;

namespace Service.Abstractions;

public interface IUserService
{
    Task<(IEnumerable<UserDto> userDtos, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, CancellationToken cancellationToken = default);
    Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserDto> CreateUserAsync(UserCreationDto userCreationDto, CancellationToken cancellationToken = default);
    Task<UserDto> CreateManagerAsync(UserCreationDto userCreationDto, CancellationToken cancellationToken = default);
    Task SetUserEnabled(string username, bool enabled);
    Task<UserDto> UpdateUserAsync(string? id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);
    Task<(IEnumerable<UserDto> userDtos, MetaData metaData)> GetSuspiciousUsersAsync(UserParameters userParameters, CancellationToken cancellationToken = default);
}
