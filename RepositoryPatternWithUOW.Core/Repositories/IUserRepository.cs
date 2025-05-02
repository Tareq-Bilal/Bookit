using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        //Task PromoteToAdminAsync(int userId);
        Task DeactivateAsync(int userId);
        Task<IEnumerable<User>> GetInactiveUsersAsync();

    }
}
