namespace Backend.Application.Features.Admin.Dto;

public class RoleWithClaimsDto
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public List<ClaimDto> Claims { get; set; } = new();
}
