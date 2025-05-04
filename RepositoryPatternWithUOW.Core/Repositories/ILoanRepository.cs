using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface ILoanRepository : IBaseRepository<Loan> 
    {
        public Task<IEnumerable<Loan>> GetLoansInDeatils();

    }
}
