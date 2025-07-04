using AutoMapper;
using Backend.Domain.Entities;
using Backend.Application.Features.Expenses.Dtos;

namespace Backend.Application.Common.Mappings
{
    internal class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<Expense, BaseExpenseDto>().ReverseMap();
            CreateMap<Expense, ExpenseAddDto>().ReverseMap();
            CreateMap<Expense, ExpenseUpdateDto>().ReverseMap();
        }
    }
}
