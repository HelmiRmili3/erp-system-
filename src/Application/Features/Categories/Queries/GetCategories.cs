using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Categories.Dto;
using Backend.Application.Features.Categories.IRepositories;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Categories.Queries;

public record GetCategoriesQuery : IRequest<Response<IEnumerable<CategoryDto>>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Response<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCategoriesQueryHandler(
        ICategoryQueryRepository categoryQueryRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _categoryQueryRepository = categoryQueryRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryQueryRepository.GetAllByFilterAsync(
            (x) => x.ParentCategoryId == null,
            "SubCategories",
            cancellationToken);

        BaseCategoryDto.SetHttpContextAccessor(_httpContextAccessor);

        var categoryDtos = categories.Select(x => x.ToDto<CategoryDto>());
        return new Response<IEnumerable<CategoryDto>>(categoryDtos, "Categories retrieved successfully");
    }
}
