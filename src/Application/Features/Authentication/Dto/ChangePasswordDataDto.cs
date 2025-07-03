namespace Backend.Application.Features.Authentication.Dto;

public class ChangePasswordDataDto : ChangePasswordDto
{
    public required string UserId { get; init; }
}
