
using Backend.Application.Features.Payrolls.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;
public class PayrollQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Payroll>(dbContext), IPayrollQueryRepository
{
}
