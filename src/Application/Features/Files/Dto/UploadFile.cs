using Microsoft.AspNetCore.Http;

namespace Backend.Application.Files.Configurations.Dto;

public record UploadFileDto
{
    public IFormFile? File { get; set; }
}

public record RemoveFileDto
{
    public string? Path { get; set; }
}
