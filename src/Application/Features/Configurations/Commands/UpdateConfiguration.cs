using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Configurations.Dto;
using Backend.Application.Features.Configurations.IRepositories;
using Backend.Domain.Entities;

namespace Backend.Application.Features.Configurations.Commands;

public record UpdateConfigurationCommand : ConfigurationUpdateDto, IRequest<Response<ConfigurationDto>>;

public class UpdateConfigurationCommandHandler(IConfigurationCommandRepository repository, IConfigurationQueryRepository queryRepository)
    : IRequestHandler<UpdateConfigurationCommand, Response<ConfigurationDto>>
{
    private readonly IConfigurationCommandRepository _repository = repository;
    private readonly IConfigurationQueryRepository _queryRepository = queryRepository;
    public async Task<Response<ConfigurationDto>> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Guard.Against.Null(request.Key, nameof(request.Key));
            Guard.Against.Null(request.Value, nameof(request.Value));

            var entity = await _queryRepository.GetSingleByFilterAsync(x => x.Key == request.Key, null, cancellationToken);


            if (entity == null)
            {
                entity = new Configuration(request.Key, request.Value);
                await _repository.AddAsync(entity, cancellationToken);
                return new Response<ConfigurationDto>(entity.ToDto<ConfigurationDto>(), "Configuration created successfully");
            }


            entity.Update(request.Value);

            await _repository.UpdateAsync(entity, cancellationToken);

            return new Response<ConfigurationDto>(entity.ToDto<ConfigurationDto>(), "Configuration updated successfully");
        }
        catch (Exception e) when (e is not NotFoundException)
        {
            throw new ApplicationException(e.Message);
        }
    }
}


public class UpdateConfigurationCommandValidator : AbstractValidator<UpdateConfigurationCommand>
{
    private readonly IConfigurationQueryRepository _repository;

    public UpdateConfigurationCommandValidator(IConfigurationQueryRepository repository)
    {
        _repository = repository;

        RuleFor(v => v.Key)
            .NotEmpty();

        RuleFor(v => v.Value)
            .NotEmpty();
    }

}
