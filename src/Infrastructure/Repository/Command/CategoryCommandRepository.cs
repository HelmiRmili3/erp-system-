using Backend.Application.Features.Categories.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;

public class CategoryCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Category>(dbContext), ICategoryCommandRepository
{
}
