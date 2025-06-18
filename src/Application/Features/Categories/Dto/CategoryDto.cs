using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Backend.Domain.Entities;

namespace Backend.Application.Features.Categories.Dto;

public class CategoryDto : BaseCategoryDto
{
    public ICollection<BaseCategoryDto>? SubCategories { get; set; }
    public BaseCategoryDto? ParentCategory { get; set; }
}

public class BaseCategoryDto
{
    private static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor(); // Default fallback

    public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    public int Id { get; init; }
    public string? Logo { get; set; }
    public string? Name { get; set; }
    public int? ParentCategoryId { get; set; }

    public string? LogoUrl
    {
        get
        {
            if (string.IsNullOrEmpty(Logo)) return null;
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return Logo; // Fallback to just Logo if no context
            return $"{request.Scheme}://{request.Host}{request.PathBase}/files/{Logo.TrimStart('/')}";
        }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, BaseCategoryDto>();
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
                .IncludeBase<Category, BaseCategoryDto>();
        }
    }
}

public record CategoryAddDto
{
    public string? Name { get; init; }
    public int? ParentCategoryId { get; init; }
    public IFormFile? File { get; init; }
}

public record CategoryUpdateDto : CategoryAddDto
{
    public int Id { get; init; }
}
