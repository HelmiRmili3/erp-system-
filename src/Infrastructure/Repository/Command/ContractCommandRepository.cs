using Backend.Application.Features.Contracts.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;
public class ContractCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Contract>(dbContext), IContractCommandRepository
{
}
