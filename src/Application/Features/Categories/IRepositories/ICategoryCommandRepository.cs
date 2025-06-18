using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.Features.Categories.IRepositories;

public interface ICategoryCommandRepository : ICommandRepository<Category>
{
}
