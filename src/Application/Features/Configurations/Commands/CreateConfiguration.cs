using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Categories.IRepositories;
using Backend.Application.Features.Configurations.Dto;
using Backend.Application.Features.Configurations.IRepositories;

namespace Backend.Application.Features.Configurations.Commands;

public record CreateConfigurationCommand : ConfigurationAddDto, IRequest<Response<ConfigurationDto>>;

public class CreateConfigurationCommandHandler(IConfigurationCommandRepository repository)
    : IRequestHandler<CreateConfigurationCommand, Response<ConfigurationDto>>
{
    private readonly IConfigurationCommandRepository _repository = repository;
    public async Task<Response<ConfigurationDto>> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Value, nameof(request.Value));
        Guard.Against.Null(request.Key, nameof(request.Key));

        var entity = new Configuration(request.Key, request.Value);

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result == null)
        {
            throw new ApplicationException("Configuration could not be created");
        }

        return new Response<ConfigurationDto>(result.ToDto<ConfigurationDto>(), "Configuration created successfully");
    }
}


public class CreateConfigurationCommandValidator : AbstractValidator<CreateConfigurationCommand>
{
    private readonly IConfigurationQueryRepository _repository;

    public CreateConfigurationCommandValidator(IConfigurationQueryRepository repository)
    {
        _repository = repository;

        RuleFor(v => v.Key)
            .NotEmpty()
            .MustAsync(async (key, cancellationToken) => !await _repository.ExistAsync(x => x.Key == key, cancellationToken))
            .WithMessage("The specified key already exists");

        RuleFor(v => v.Value)
            .NotEmpty();
    }
}
