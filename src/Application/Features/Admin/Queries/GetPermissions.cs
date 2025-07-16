using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetPermissionsQuery() : IRequest<Response<List<PermissionDto>>>;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, Response<List<PermissionDto>>>
{
    private readonly IAdminQueryRepository _adminQueryRepository;

    public GetPermissionsQueryHandler(IAdminQueryRepository adminQueryRepository)
    {
        _adminQueryRepository = adminQueryRepository;
    }

    public async Task<Response<List<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _adminQueryRepository.GetPermissionsAsync(cancellationToken);
    }
}
