using AutoMapper;
using Backend.Domain.Entities;
using Backend.Application.Features.Payrolls.Dtos;

namespace Backend.Application.Common.Mappings
{
    internal class PayrollProfile : Profile
    {
        public PayrollProfile()
        {
            CreateMap<Payroll, PayrollDto>().ReverseMap();
            CreateMap<Payroll, BasePayrollDto>().ReverseMap();
            CreateMap<Payroll, PayrollAddDto>().ReverseMap();
        }
    }
}
