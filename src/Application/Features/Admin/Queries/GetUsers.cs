using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;

namespace Backend.Application.Features.Admin.Queries;

public record GetUsersQuery(PagingParameter Paging) : IRequest<PagedResponse<List<UserDto>>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResponse<List<UserDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetUsersQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUsersWithRolesAndPermissionsAsync(
            request.Paging.PageNumber,
            request.Paging.PageSize,
            cancellationToken);
    }
}
