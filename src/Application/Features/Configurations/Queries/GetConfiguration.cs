using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Configurations.Dto;
using Backend.Application.Features.Configurations.IRepositories;

namespace Backend.Application.Features.Configurations.Queries;

public record GetConfigurationQuery(string Key) : IRequest<Response<ConfigurationDto>>;

public class GetConfigurationQueryHandler(IConfigurationQueryRepository repository)
    : IRequestHandler<GetConfigurationQuery, Response<ConfigurationDto>>
{
    private readonly IConfigurationQueryRepository _repository = repository;
    public async Task<Response<ConfigurationDto>> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetSingleByFilterAsync(x => x.Key == request.Key, null, cancellationToken);

        Guard.Against.NotFound(request.Key, entity);

        return new Response<ConfigurationDto>(entity.ToDto<ConfigurationDto>(), "Configuration retrieved successfully");
    }
}

public class GetConfigurationQueryValidator : AbstractValidator<GetConfigurationQuery>
{
    public GetConfigurationQueryValidator()
    {
        RuleFor(v => v.Key)
            .NotEmpty().WithMessage("Id is required.");
    }
}
