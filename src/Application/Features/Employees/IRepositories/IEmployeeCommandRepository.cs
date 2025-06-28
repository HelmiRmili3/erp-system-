using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
namespace Backend.Application.Features.Employees.IRepositories;
public interface IEmployeeCommandRepository : ICommandRepository<Employee>
{
}
