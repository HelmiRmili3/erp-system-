using AutoMapper;
using Backend.Application.Features.Certifications.Dto;
using Backend.Domain.Entities;

namespace Backend.Application.Common.Mappings;

public class CertificationProfile : Profile
{
    public CertificationProfile()
    {
        // Entity <-> DTO
        CreateMap<Certification, CertificationDto>().ReverseMap();

        // CreateCertificationDto -> Certification (one way, no reverse map needed)
        CreateMap<CreateCertificationDto, Certification>();

        // UpdateCertificationDto -> Certification (one way, no reverse map needed)
        CreateMap<UpdateCertificationDto, Certification>();
    }
}
