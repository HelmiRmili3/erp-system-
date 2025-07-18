using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;

namespace Backend.Application.Features.Admin.Queries;

public record GetRolesQuery(PagingParameter PagingParameter)
    : IRequest<PagedResponse<List<RoleDto>>>;

public class GetRolesQueryHandler
    : IRequestHandler<GetRolesQuery, PagedResponse<List<RoleDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetRolesQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PagingParameter.PageNumber;
        var pageSize = request.PagingParameter.PageSize;
        return await _repository.GetRolesAsync(pageNumber, pageSize, cancellationToken);
    }
}
