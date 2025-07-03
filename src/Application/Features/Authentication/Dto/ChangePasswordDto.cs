namespace Backend.Application.Features.Authentication.Dto;

public class ChangePasswordDto
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmNewPassword { get; init; }
}
