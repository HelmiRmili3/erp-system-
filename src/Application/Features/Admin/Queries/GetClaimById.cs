using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;

namespace Backend.Application.Features.Admin.Queries;

public record GetClaimByIdQuery(int Id) : IRequest<Response<ClaimDto>>;

public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, Response<ClaimDto>>
{
    private readonly IAdminQueryRepository _adminQueryRepository;

    public GetClaimByIdQueryHandler(IAdminQueryRepository adminQueryRepository)
    {
        _adminQueryRepository = adminQueryRepository;
    }

    public async Task<Response<ClaimDto>> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
    {
        return await _adminQueryRepository.GetClaimByIdAsync(request.Id, cancellationToken);
    }
}
