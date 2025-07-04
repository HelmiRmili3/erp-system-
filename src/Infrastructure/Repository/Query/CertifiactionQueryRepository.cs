using Backend.Application.Features.Certifications.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Query.Base;
namespace Backend.Infrastructure.Repository.Query;
public class CertifiactionQueryRepository(ApplicationDbContext dbContext) : QueryRepository<Certification>(dbContext), ICertificationQueryRepository
{
}
