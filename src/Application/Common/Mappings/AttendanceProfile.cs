using Backend.Application.Features.Attendances.Dto;

namespace Backend.Application.Common.Mappings
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<Attendance, AttendanceDto>().ReverseMap();
            CreateMap<Attendance, AttendanceAddDto>().ReverseMap();
            CreateMap<Attendance, AttendanceUpdateDto>().ReverseMap();
        }
    }
}
