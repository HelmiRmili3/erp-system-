using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;
public class AttendanceCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Attendance>(dbContext), IAttendanceCommandRepository
{
}
