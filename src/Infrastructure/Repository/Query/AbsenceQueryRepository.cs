using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;

namespace Backend.Infrastructure.Repository.Query;

public class AbsenceQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Absence>(dbContext), IAbsenceQueryRepository
{
}
