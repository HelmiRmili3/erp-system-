using AutoMapper;
using Backend.Application.Features.Absences.Dto;
using Backend.Domain.Entities;

namespace Backend.Application.Common.Mappings;

public class AbsenceProfile : Profile
{
    public AbsenceProfile()
    {
        CreateMap<Absence, AbsenceDto>().ReverseMap();
    }
}
