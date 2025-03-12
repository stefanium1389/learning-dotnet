using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using Mapster;
using Shared.Dtos;
using Shared.RequestFeatures;
using System.Data.Common;
using Domain.Exceptions;

namespace Service;

internal sealed class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;

    public UserService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<UserDto> CreateUserAsync(UserCreationDto userCreationDto, CancellationToken cancellationToken = default)
    {
        var user = userCreationDto.Adapt<User>();
        user.Role = Role.Tourist;
        string id = await _repositoryManager.KeycloakRepository.CreateKeycloakUserAsync(user);
        user.Id = new Guid(id);
        await _repositoryManager.KeycloakRepository.AssignKeycloakUserRole(user);
        await _repositoryManager.UserRepository.InsertAsync(user, cancellationToken);

        return user.Adapt<UserDto>();
    }

    public async Task<UserDto> CreateManagerAsync(UserCreationDto userCreationDto, CancellationToken cancellationToken = default)
    {
        var user = userCreationDto.Adapt<User>();
        user.Role = Role.Manager;
        string id = await _repositoryManager.KeycloakRepository.CreateKeycloakUserAsync(user);
        user.Id = new Guid(id);
        await _repositoryManager.KeycloakRepository.AssignKeycloakUserRole(user);
        await _repositoryManager.UserRepository.InsertAsync(user, cancellationToken);

        return user.Adapt<UserDto>();
    }

    public async Task<(IEnumerable<UserDto> userDtos, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, CancellationToken cancellationToken = default)
    {
        var usersWithMetaData = await _repositoryManager.UserRepository.GetAllAsync(userParameters, cancellationToken);
        var usersDto = usersWithMetaData.Adapt<IEnumerable<UserDto>>();
        return (usersDto, usersWithMetaData.MetaData);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, cancellationToken);
        return user.Adapt<UserDto>();
    }

    public async Task SetUserEnabled(string username, bool enabled)
    {
        await _repositoryManager.KeycloakRepository.SetUserEnabled(username, enabled);
    }

    public async Task<UserDto> UpdateUserAsync(string? id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(Guid.Parse(id!), cancellationToken);
        user.Name = userUpdateDto.Name ?? user.Name;
        user.Lastname = userUpdateDto.Lastname ?? user.Lastname;
        if(userUpdateDto.DateOfBirth != DateOnly.FromDayNumber(0))
        {
            user.DateOfBirth = userUpdateDto.DateOfBirth;
        }
        user.Email = userUpdateDto.Email ?? user.Email;
        if(userUpdateDto.Gender is not null)
        {
            try
            {
                user.Gender = Enum.Parse<Gender>(userUpdateDto.Gender);
            }
            catch
            {
                throw new InvalidEnumValueExcepton(userUpdateDto.Gender, "Gender");
            }
        }
        await _repositoryManager.KeycloakRepository.UpdateKeycloakUser(user);
        await _repositoryManager.UserRepository.UpdateAsync(user, cancellationToken);
        return user.Adapt<UserDto>();
    }

    public async Task<(IEnumerable<UserDto> userDtos, MetaData metaData)> GetSuspiciousUsersAsync(UserParameters userParameters, CancellationToken cancellationToken = default)
    {
        var usersWithMetaData = await _repositoryManager.UserRepository.GetSuspiciousUsersAsync(userParameters, cancellationToken);
        var usersDto = usersWithMetaData.Adapt<IEnumerable<UserDto>>();
        return (usersDto, usersWithMetaData.MetaData);
    }
}
