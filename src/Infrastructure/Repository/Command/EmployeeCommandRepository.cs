using Backend.Application.Features.Employees.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;
public class EmployeeCommandRepository(ApplicationDbContext dbContext):CommandRepository<Employee>(dbContext), IEmployeeCommandRepository
{
}
