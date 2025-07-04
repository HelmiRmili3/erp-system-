
using Backend.Application.Features.Contracts.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;
public class ContractQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Contract>(dbContext), IContractQueryRepository
{
}
