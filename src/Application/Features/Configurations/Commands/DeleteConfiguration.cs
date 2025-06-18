using Backend.Application.Common.Response;
using Backend.Application.Features.Configurations.IRepositories;

namespace Backend.Application.Features.Configurations.Commands;

public record DeleteConfigurationCommand(string Key) : IRequest<Response<string>>;

public class DeleteConfigurationCommandHandler(IConfigurationCommandRepository repository, IConfigurationQueryRepository queryRepository)
    : IRequestHandler<DeleteConfigurationCommand, Response<string>>
{
    private readonly IConfigurationCommandRepository _repository = repository;
    private readonly IConfigurationQueryRepository _queryRepository = queryRepository;

    public async Task<Response<string>> Handle(DeleteConfigurationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _queryRepository.GetSingleByFilterAsync(x => x.Key == request.Key, null, cancellationToken);

            Guard.Against.NotFound(request.Key, entity);

            await _repository.DeleteAsync(entity, cancellationToken);

            return new Response<string>("Configuration deleted successfully", request.ToString());
        }
        catch (Exception e) when (e is not NotFoundException)
        {
            throw new ApplicationException(e.Message);
        }
    }
}
