using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;
namespace Backend.Infrastructure.Repository.Query;
public class AttendanceQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Attendance>(dbContext), IAttendanceQueryRepository
{
}
