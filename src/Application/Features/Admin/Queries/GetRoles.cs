using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetRolesQuery : IRequest<Response<List<RoleDto>>>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Response<List<RoleDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetRolesQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetRolesAsync(cancellationToken);
    }
}
