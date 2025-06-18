using Backend.Application.Features.Configurations.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;

public class ConfigurationQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Configuration>(dbContext), IConfigurationQueryRepository
{
}
