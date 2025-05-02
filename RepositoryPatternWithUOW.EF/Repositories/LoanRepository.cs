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
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetLoansInDeatils()
        {
            return await _context.Loans
                                      .Include(l => l.Book)
                                      .Include(l => l.User)
                                      .ToListAsync(); 

        }
    }
}
