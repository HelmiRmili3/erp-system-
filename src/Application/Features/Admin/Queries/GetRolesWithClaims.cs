using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetRolesWithClaimsQuery : IRequest<Response<List<RoleWithClaimsDto>>>;

public class GetRolesWithClaimsQueryHandler : IRequestHandler<GetRolesWithClaimsQuery, Response<List<RoleWithClaimsDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetRolesWithClaimsQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<RoleWithClaimsDto>>> Handle(GetRolesWithClaimsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetRolesWithClaimsAsync(cancellationToken);
    }
}
