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
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionReposirtory
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, decimal>> AmountByTypeAsync()
        {
            return await _context.Transactions
                                       .GroupBy(t => t.TransactionType)
                                       .Select(g => new
                                       {
                                           TransactionType = g.Key,
                                           TotalAmount = g.Sum(t => t.Amount)
                                       })
                                       .ToDictionaryAsync(
                                          x => x.TransactionType,
                                          x => x.TotalAmount
                                       );
        
        }

        public async Task<Dictionary<string, int>> CountByTypeAsync()
        {
            return await _context.Transactions.GroupBy(t => t.TransactionType)
                                       .Select(g => new
                                       {
                                           TransactionType  = g.Key,
                                           TransactionCount = g.Count()
                                       })
                                       .ToDictionaryAsync(
                                          x => x.TransactionType,
                                          x => x.TransactionCount
                                       );
        }

        public async Task<decimal> GetTotalTransactionsAmountAsync()
        {
            return await _context.Transactions.SumAsync(t => t.Amount);
        }
    }
}
