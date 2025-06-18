using Backend.Application.Features.Configurations.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;

public class ConfigurationCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Configuration>(dbContext), IConfigurationCommandRepository
{
}
