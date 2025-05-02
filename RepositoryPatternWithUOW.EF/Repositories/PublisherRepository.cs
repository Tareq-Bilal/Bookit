using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class PublisherRepository : BaseRepository<Publisher>, IPublisherRepository
    {
        private readonly ApplicationDbContext _context;

        public PublisherRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Publisher> GetPublisherDetailsAsync(int id)
        {
            return await _context.Publishers
                                 .Include(p => p.Books)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publisher?> GetPublisherWithBooksAsync(int id)
        {
            return await _context.Publishers
                                 .Include(p => p.Books)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<string> GetPublisherNameByID(int id)
        {
            return await _context.Publishers
                                  .Where( p => p.Id == id)
                                  .Select(p => p.Name)
                                  .SingleOrDefaultAsync();

        }


        public async Task<IDictionary<int, int>> GetYearlyPublicationCountsAsync(int publisherId, int startYear, int endYear)
        {
              var result = await _context.Books
            .Where(b => b.PublisherId == publisherId &&
                        b.PublicationDate.Value.Year >= startYear &&
                        b.PublicationDate.Value.Year <= endYear)
            .GroupBy(b => b.PublicationDate.Value.Year)
            .Select(g => new
            {
                Year = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(x => x.Year, x => x.Count);

              return result;

        }

        public async Task<IEnumerable<Publisher>> SearchPublishersByNameAsync(string searchTerm)
        {
            return await _context.Publishers.Where(p => p.Name.Trim().ToLower().Contains(searchTerm.ToLower())).ToListAsync();
        }

    }
}
