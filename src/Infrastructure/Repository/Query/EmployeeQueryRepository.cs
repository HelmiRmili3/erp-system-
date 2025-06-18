using Backend.Application.Features.Employees.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;
public class EmployeeQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Employee>(dbContext), IEmployeeQueryRepository
{
}
