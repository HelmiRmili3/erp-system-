using Backend.Application.Features.Absences.Dto;

namespace Backend.Application.Common.Mappings;

public class AbsenceProfile : Profile
{
    public AbsenceProfile()
    {
        CreateMap<Absence, AbsenceDto>().ReverseMap();
    }
}
