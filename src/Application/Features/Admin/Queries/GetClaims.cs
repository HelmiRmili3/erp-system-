using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetClaimsQuery() : IRequest<Response<List<ClaimDto>>>;

public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, Response<List<ClaimDto>>>
{
    private readonly IAdminQueryRepository _adminQueryRepository;

    public GetClaimsQueryHandler(IAdminQueryRepository adminQueryRepository)
    {
        _adminQueryRepository = adminQueryRepository;
    }

    public async Task<Response<List<ClaimDto>>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        return await _adminQueryRepository.GetClaimsAsync(cancellationToken);
    }
}
