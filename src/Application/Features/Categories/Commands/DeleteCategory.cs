using Backend.Application.Common.Response;
using Backend.Application.Features.Categories.IRepositories;
using Backend.Domain.Events.Categories;

namespace Backend.Application.Features.Categories.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<Response<string>>;

public class DeleteCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, ICategoryQueryRepository categoryQueryRepository)
    : IRequestHandler<DeleteCategoryCommand, Response<string>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository = categoryCommandRepository;
    private readonly ICategoryQueryRepository _categoryQueryRepository = categoryQueryRepository;
    public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _categoryQueryRepository.GetByIdAsync(request.Id, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            await _categoryCommandRepository.DeleteAsync(entity, cancellationToken);

            entity.AddDomainEvent(new CategoryDeletedEvent(entity));

            return new Response<string>("Category deleted successfully", request.ToString());
        }
        catch (Exception e) when (e is not NotFoundException)
        {
            throw new ApplicationException(e.Message);
        }
    }

}
