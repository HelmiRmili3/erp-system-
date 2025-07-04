
using Backend.Application.Features.Expenses.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;
public class ExpenseCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Expense>(dbContext), IExpenseCommandRepository
{
}
