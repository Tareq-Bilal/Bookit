using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;
        private TransactionType.enTransactionType _transactionType;
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

        public async Task<Transaction> ProcessBookReturnAsync(int loanId, BookCopyReturnDTO dto , IUnitOfWork unitOfWork)
        {
            var loan = await _context.Loans.SingleOrDefaultAsync(l => l.Id == loanId);
            var copy = await _context.BookCopies.SingleOrDefaultAsync(c => c.Id == loan.BookCopyId);

                loan.ReturnDate = dto.ReturnDate;
                loan.Status = LoanStatus.enStatus.Returned.ToString();
                copy.Status = BookCopyStatus.enStatus.Available.ToString();

            // Calculate Loan Fines And Add Transaction
            var Transaction = new Transaction
            {
                UserId = loan.UserId,
                LoanId = loanId,
                Amount = _CalculateLoanFines(loanId, dto, unitOfWork),
                BookId = loan.BookId,
                TransactionDate = DateTime.Now,
                TransactionType = _transactionType.ToString(),
                Description = null
            };

            _context.Transactions.Add(Transaction);
            _context.BookCopies.Update(copy);
            _context.Loans.Update(loan);
            _context.SaveChanges();
            
            return Transaction;
        }

        private decimal _CalculateLoanFines(int loanId , BookCopyReturnDTO dto,IUnitOfWork unitOfWork)
        {
            decimal Fines = 0;
            var loan = _context.Loans.SingleOrDefault(l => l.Id == loanId);

            //Overdue Case
            if (loan?.ReturnDate.HasValue == true && loan.DueDate < loan.ReturnDate.Value)
            {
                var lateReturnFee = unitOfWork.Settings.GetLateReturnFee(); // await if async
                var daysLate = (loan.ReturnDate.Value - loan.DueDate).Days;
                _transactionType = TransactionType.enTransactionType.Fine;
                Fines = daysLate * lateReturnFee;
            }

            //Damage Case
            if (dto.Condition == BookReturnCondition.enCondition.Damaged.ToString())
                Fines += dto.AdditionalCharges;

            //Refund Case
            if (loan?.ReturnDate.HasValue == true && loan.ReturnDate.Value < loan.DueDate)
            {
                var earlyReturnRefundRate = unitOfWork.Settings.GetLateReturnFee(); // Fee per day for early returns
                var daysEarly = (loan.DueDate - loan.ReturnDate.Value).Days;
                _transactionType = TransactionType.enTransactionType.Refund;
                Fines = daysEarly * earlyReturnRefundRate;
            }

             return Fines;
        }

    }
}
