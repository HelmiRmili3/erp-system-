using AutoMapper;
using Backend.Domain.Entities;
using Backend.Application.Features.Contracts.Dtos;

namespace Backend.Application.Common.Mappings;

internal class ContractProfile : Profile
{
    public ContractProfile()
    {
        // Entity -> DTO
        CreateMap<Contract, ContractDto>();

        // DTO -> Entity for create
        CreateMap<ContractAddDto, Contract>();

        // DTO -> Entity for update
        CreateMap<ContractUpdateDto, Contract>();
    }
}
