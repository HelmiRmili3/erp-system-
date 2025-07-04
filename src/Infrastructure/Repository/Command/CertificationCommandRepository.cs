using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Application.Features.Certifications.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;

namespace Backend.Infrastructure.Repository.Command;
internal class CertificationCommandRepository(ApplicationDbContext dbContext) : CommandRepository<Certification>(dbContext), ICertificationCommandRepository
{
}
