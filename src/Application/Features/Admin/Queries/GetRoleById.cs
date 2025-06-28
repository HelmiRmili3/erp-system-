using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using MediatR;

namespace Backend.Application.Features.Admin.Queries;

public record GetRoleByIdQuery(string RoleId) : IRequest<Response<RoleDto>>;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Response<RoleDto>>
{
    private readonly IAdminQueryRepository _repository;

    public GetRoleByIdQueryHandler(IAdminQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetRoleByIdAsync(request.RoleId, cancellationToken);
    }
}
