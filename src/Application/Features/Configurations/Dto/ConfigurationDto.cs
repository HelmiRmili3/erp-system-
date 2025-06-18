using Backend.Domain.Entities;

namespace Backend.Application.Features.Configurations.Dto;

public class ConfigurationDto
{
    public int Id { get; init; }
    public string? Key { get; set; }
    public string? Value { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Configuration, ConfigurationDto>();
        }
    }
}

public record ConfigurationAddDto
{
    public string? Key { get; init; }
    public string? Value { get; init; }
}

public record ConfigurationUpdateDto : ConfigurationAddDto
{
}
