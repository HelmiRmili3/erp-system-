using Backend.Application.Features.User.Dto;
using Backend.Application.Features.User.IRepositories;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repository.User
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserQueryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDataDto?> GetByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) return null;

            return new UserDataDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                FileUrl = user.FileUrl!
            };
        }
    }
}
