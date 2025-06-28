namespace Backend.Application.Features.Admin.Dto;

public class ClaimWithRolesDto
{
    public int ClaimId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<RoleDto> Roles { get; set; } = new();
}
