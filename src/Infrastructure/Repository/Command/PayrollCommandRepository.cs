
using Backend.Application.Features.Payrolls.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;
public class PayrollCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Payroll>(dbContext), IPayrollCommandRepository
{
}
