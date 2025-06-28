using Backend.Application.Common.Interfaces;
namespace Backend.Application.Features.Absences.IRepositories;

public interface IAbsenceCommandRepository : ICommandRepository<Absence>
{
}
