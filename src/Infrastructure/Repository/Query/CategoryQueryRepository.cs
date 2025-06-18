using Backend.Application.Features.Categories.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;

public class CategoryQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Category>(dbContext), ICategoryQueryRepository
{
}
