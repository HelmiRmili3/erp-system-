using Microsoft.AspNetCore.Http;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Categories.Dto;
using Backend.Application.Features.Categories.IRepositories;
using Backend.Domain.Events.Categories;

namespace Backend.Application.Features.Categories.Commands;

public record UpdateCategoryCommand : CategoryUpdateDto, IRequest<Response<CategoryDto>>;

public class UpdateCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, ICategoryQueryRepository categoryQueryRepository, IFileService fileService)
    : IRequestHandler<UpdateCategoryCommand, Response<CategoryDto>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository = categoryCommandRepository;
    private readonly ICategoryQueryRepository _categoryQueryRepository = categoryQueryRepository;

    private readonly IFileService _fileService = fileService;
    public async Task<Response<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _categoryQueryRepository.GetByIdAsync(request.Id, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Update(request.Name, request.ParentCategoryId, await HandleLogoUpdateAsync(request.File, entity.Logo));

            entity.AddDomainEvent(new CategoryUpdatedEvent(entity));

            await _categoryCommandRepository.UpdateAsync(entity, cancellationToken);

            return new Response<CategoryDto>(entity.ToDto<CategoryDto>(), "Category updated successfully");
        }
        catch (Exception e) when (e is not NotFoundException)
        {
            throw new ApplicationException(e.Message);
        }
    }

    private async Task HandleLogoDeleteAsync(string? logo)
    {
        if (logo != null)
        {
            await _fileService.DeleteFileAsync(logo);
        }
    }

    private async Task<string> HandleLogoUploadAsync(IFormFile? logo)
    {
        return logo == null
            ? string.Empty
            : await _fileService.SaveFileAsync(logo, "category_" + Guid.NewGuid().ToString(), "categories");
    }

    private async Task<string?> HandleLogoUpdateAsync(IFormFile? logo, string? oldLogo)
    {
        if (logo != null)
        {
            await HandleLogoDeleteAsync(oldLogo);
            return await HandleLogoUploadAsync(logo);
        }

        return oldLogo;
    }
}


public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryQueryRepository categoryQueryRepository)
    {

        RuleFor(v => v.Id)
            .NotEmpty()
            .MustAsync((id, cancellationToken) => categoryQueryRepository.ExistAsync(x => x.Id == id, cancellationToken))
            .WithMessage("Category does not exist");

        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();


        RuleFor(v => v.ParentCategoryId)
            .MustAsync((id, cancellationToken) => categoryQueryRepository.ExistAsync(x => x.Id == id!.Value, cancellationToken))
            .When(v => v.ParentCategoryId != null)
            .WithMessage("Parent category does not exist");
    }
}
