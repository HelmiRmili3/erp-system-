
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Features.Expenses.IRepositories;
public interface IExpenseQueryRepository: IQueryRepository<Expense>
{
}
