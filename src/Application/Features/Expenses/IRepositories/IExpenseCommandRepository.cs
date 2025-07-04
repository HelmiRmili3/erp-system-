using Backend.Application.Common.Interfaces;

namespace Backend.Application.Features.Expenses.IRepositories;
public interface IExpenseCommandRepository : ICommandRepository<Expense>
{
}
