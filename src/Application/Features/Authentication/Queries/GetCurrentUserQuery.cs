using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;

namespace Backend.Application.Features.Authentication.Queries;

public record GetCurrentUserQuery(string UserId) : IRequest<Response<RegisterResultDto>>;

public class GetCurrentUserQueryHandler(IAuthenticationQueryRepository repository)
    : IRequestHandler<GetCurrentUserQuery, Response<RegisterResultDto>>
{
    private readonly IAuthenticationQueryRepository _repository = repository;

    public async Task<Response<RegisterResultDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUserById(request.UserId);
    }
}
