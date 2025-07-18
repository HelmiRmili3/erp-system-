using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;

namespace Backend.Application.Features.Admin.Queries;

public record GetRolesWithPermissionsQuery(PagingParameter PagingParameter)
    : IRequest<PagedResponse<List<RoleWithPermissionsDto>>>;

public class GetRolesWithPermissionsQueryHandler
    : IRequestHandler<GetRolesWithPermissionsQuery, PagedResponse<List<RoleWithPermissionsDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetRolesWithPermissionsQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<RoleWithPermissionsDto>>> Handle(
        GetRolesWithPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = request.PagingParameter.PageNumber;
        var pageSize = request.PagingParameter.PageSize;
        return await _repository.GetRolesWithPermissionsAsync(
            pageNumber,
            pageSize,
            cancellationToken);
    }
}
