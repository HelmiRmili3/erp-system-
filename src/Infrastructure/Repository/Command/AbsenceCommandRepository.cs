
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;

public class AbsenceCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Absence>(dbContext), IAbsenceCommandRepository
{
}
