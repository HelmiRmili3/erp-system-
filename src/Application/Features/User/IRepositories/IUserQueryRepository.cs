using Backend.Application.Features.User.Dto;

namespace Backend.Application.Features.User.IRepositories;

public interface IUserQueryRepository
{
    Task<UserDataDto?> GetByIdAsync(string userId, CancellationToken cancellationToken);
}
