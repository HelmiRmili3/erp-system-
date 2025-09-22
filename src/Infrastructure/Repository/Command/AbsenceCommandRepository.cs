using System.Threading;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Enums;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;

public class AbsenceCommandRepository : CommandRepository<Absence>, IAbsenceCommandRepository
{
    private readonly ApplicationDbContext _dbContext;


    public AbsenceCommandRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ApproveAbsenceAsync(int absenceId)
    {
        var absence = await _dbContext.Set<Absence>().FindAsync(absenceId);

        if (absence == null) return;

        absence.StatusType = AbsenceStatus.Approved;
        await _dbContext.SaveChangesAsync();
    }

    public async Task RejectAbsenceAsync(int absenceId)
    {
        var absence = await _dbContext.Set<Absence>().FindAsync(absenceId);
        if (absence == null) return;

        absence.StatusType = AbsenceStatus.Rejected;
        await _dbContext.SaveChangesAsync();
    }
}
