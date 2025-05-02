using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IPublisherRepository : IBaseRepository<Publisher> 
    {
        Task<Publisher> GetPublisherDetailsAsync(int id);
        Task<IEnumerable<Publisher>> SearchPublishersByNameAsync(string searchTerm);
        public Task<Publisher?> GetPublisherWithBooksAsync(int id);
        public Task<string> GetPublisherNameByID(int id);

        //// Reporting operationss
        Task<IDictionary<int, int>> GetYearlyPublicationCountsAsync(int publisherId, int startYear, int endYear);
    }
}
