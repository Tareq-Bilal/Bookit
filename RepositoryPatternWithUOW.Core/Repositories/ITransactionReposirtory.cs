using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface ITransactionReposirtory : IBaseRepository<Transaction>
    {
        Task<decimal> GetTotalTransactionsAmountAsync();
        Task<Dictionary<string, int>> CountByTypeAsync();
        Task<Dictionary<string, decimal>> AmountByTypeAsync();

    }
}
