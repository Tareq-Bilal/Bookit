using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeactivateAsync(int userId)
        {

            var user = await _context.Users.FindAsync(userId);
            user.IsActive = false;
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync()
        {
            return await _context.Users.Where(u => u.IsActive == false).ToListAsync();
        }
    }
}
