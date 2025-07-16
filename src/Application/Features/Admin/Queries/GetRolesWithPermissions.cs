using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;

namespace Backend.Application.Features.Admin.Queries;

public record GetRolesWithPermissionsQuery : IRequest<Response<List<RoleWithPermissionsDto>>>;

public class GetRolesWithPermissionsQueryHandler : IRequestHandler<GetRolesWithPermissionsQuery, Response<List<RoleWithPermissionsDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetRolesWithPermissionsQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<RoleWithPermissionsDto>>> Handle(GetRolesWithPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetRolesWithPermissionsAsync(cancellationToken);
    }
}
