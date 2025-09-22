namespace Backend.Application.Features.User.Dto
{
    public class UserDataDto
    {
        public string? Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FileUrl { get; set; } // Optional profile picture or related file
    }
}
