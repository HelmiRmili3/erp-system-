using Microsoft.AspNetCore.Http;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Common.Security;
using Backend.Application.Features.Categories.Dto;
using Backend.Application.Features.Categories.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Events.Categories;

namespace Backend.Application.Features.Categories.Commands;

public record CreateCategoryCommand : CategoryAddDto, IRequest<Response<CategoryDto>>;

public class CreateCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, IFileService fileService)
    : IRequestHandler<CreateCategoryCommand, Response<CategoryDto>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository = categoryCommandRepository;
    private readonly IFileService _fileService = fileService;

    public async Task<Response<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var logo = await HandleLogoUploadAsync(request.File);
        var entity = new Category
        {
            Logo = logo,
            Name = request.Name,
            ParentCategoryId = request.ParentCategoryId,
        };

        entity.AddDomainEvent(new CategoryCreatedEvent(entity));

        var created = await _categoryCommandRepository.AddAsync(entity, cancellationToken);
        if (created == null)
        {
            throw new ApplicationException("Category could not be created");
        }

        return new Response<CategoryDto>(created.ToDto<CategoryDto>(), "Category created successfully");
    }

    private async Task<string> HandleLogoUploadAsync(IFormFile? file)
    {
        return file == null
            ? string.Empty
            : await _fileService.SaveFileAsync(file, "category_" + Guid.NewGuid().ToString(), "categories");
    }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryQueryRepository categoryQueryRepository)
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(v => v.File)
            .NotNull()
            .NotEmpty()
            .WithMessage("Logo is required");

        RuleFor(v => v.ParentCategoryId)
            .MustAsync(async (id, cancellationToken) => await categoryQueryRepository.ExistAsync(x => x.Id == id, cancellationToken))
            .When(v => v.ParentCategoryId != null)
            .WithMessage("Parent category does not exist");
    }
}
