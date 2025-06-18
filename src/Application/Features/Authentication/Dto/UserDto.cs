using Backend.Application.Common.Interfaces;

namespace Backend.Application.Features.Authentication.Dto
{
    public class UserDto : IUser
    {
        public required string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public required string Email { get; set; } 
        public required string Password { get; set; }
    }
}
