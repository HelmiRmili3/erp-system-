
using Backend.Application.Features.Expenses.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;
public class ExpenseQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Expense>(dbContext), IExpenseQueryRepository
{
}
