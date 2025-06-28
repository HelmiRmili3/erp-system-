namespace Backend.Application.Features.Admin.Dto;

public class ClaimDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
