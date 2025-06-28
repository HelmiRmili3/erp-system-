using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetClaimsWithRolesQuery() : IRequest<Response<List<ClaimWithRolesDto>>>;

public class GetClaimsWithRolesQueryHandler : IRequestHandler<GetClaimsWithRolesQuery, Response<List<ClaimWithRolesDto>>>
{
    private readonly IAdminQueryRepository _repository;

    public GetClaimsWithRolesQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<ClaimWithRolesDto>>> Handle(GetClaimsWithRolesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetClaimsWithRolesAsync(cancellationToken);
    }
}
