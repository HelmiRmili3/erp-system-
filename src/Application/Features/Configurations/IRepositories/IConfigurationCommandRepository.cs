using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.Features.Configurations.IRepositories;

public interface IConfigurationCommandRepository : ICommandRepository<Configuration>
{
}
