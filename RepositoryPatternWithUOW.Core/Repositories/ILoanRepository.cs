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
        public Task<Transaction> ProcessBookReturnAsync(int loanId, BookCopyReturnDTO dto, IUnitOfWork unitOfWork);

    }

    public class BookCopyReturnDTO
    {
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public string Condition { get; set; } // Optional - if tracking condition on return
        public string? Notes { get; set; }
        public decimal AdditionalCharges { get; set; } = 0;

    }
}
