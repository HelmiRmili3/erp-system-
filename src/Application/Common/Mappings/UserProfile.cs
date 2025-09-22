using AutoMapper;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.Commands;

namespace Backend.Application.Common.Mappings
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            // Map RegisterUserRequest -> RegisterDto
            CreateMap<RegisterUserRequest, RegisterDto>()
                .ForMember(dest => dest.FileUrl, opt => opt.Ignore()); // FileUrl is handled separately

            // Optional: if you ever need reverse mapping
            // CreateMap<RegisterDto, RegisterUserRequest>();
        }
    }
}
