using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;

namespace Backend.Application.Features.Admin.Queries;

public record GetPermissionsQuery(PagingParameter PagingParameter)
    : IRequest<PagedResponse<List<PermissionDto>>>;
public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, PagedResponse<List<PermissionDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetPermissionsQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PagingParameter.PageNumber;
        var pageSize = request.PagingParameter.PageSize;

        return await _repository.GetPermissionsWithPaginationAsync(pageNumber, pageSize, cancellationToken);
    }
}

