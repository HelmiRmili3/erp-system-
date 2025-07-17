using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetUsersQuery : IRequest<Response<List<UserDto>>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Response<List<UserDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetUsersQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUsersWithRolesAndPermissionsAsync(cancellationToken);
    }
}
