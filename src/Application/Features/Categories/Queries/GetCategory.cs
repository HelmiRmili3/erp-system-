using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Categories.Dto;
using Backend.Application.Features.Categories.IRepositories;

namespace Backend.Application.Features.Categories.Queries;

public record GetCategoryQuery(int Id) : IRequest<Response<CategoryDto>>;

public class GetCategoryQueryHandler(ICategoryQueryRepository categoryQueryRepository) : IRequestHandler<GetCategoryQuery, Response<CategoryDto>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository = categoryQueryRepository;

    public async Task<Response<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var entity = await _categoryQueryRepository.GetByIdWithIncludeAsync(
            (x) => x.Id == request.Id,
            "SubCategories,ParentCategory",
            cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        return new Response<CategoryDto>(entity.ToDto<CategoryDto>(), "Category retrieved successfully");
    }
}

public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
