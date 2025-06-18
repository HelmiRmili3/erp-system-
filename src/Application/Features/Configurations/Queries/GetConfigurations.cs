using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Common.Security;
using Backend.Application.Features.Configurations.Dto;
using Backend.Application.Features.Configurations.IRepositories;

namespace Backend.Application.Features.Configurations.Queries;

public record GetConfigurationsQuery : IRequest<Response<IEnumerable<ConfigurationDto>>>;

public class GetConfigurationsQueryHandler(IConfigurationQueryRepository repository) : IRequestHandler<GetConfigurationsQuery, Response<IEnumerable<ConfigurationDto>>>
{
    private readonly IConfigurationQueryRepository _repository = repository;
    public async Task<Response<IEnumerable<ConfigurationDto>>> Handle(GetConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        return new Response<IEnumerable<ConfigurationDto>>(entities.Select(x => x.ToDto<ConfigurationDto>()), "Configuration retrieved successfully");
    }
}
