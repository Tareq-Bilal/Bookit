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
    public class CategoryRepository : BaseRepository<Category> , ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Add this to your CategoryRepository class
        // In your CategoryRepository class
        public async Task<bool> ExistsAsync(int id)
        {
            // Use AnyAsync which is more efficient for existence checks
            // And crucially, ConfigureAwait(false) to avoid deadlocks
            return await _context.Set<Category>()
                .AnyAsync(c => c.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.Set<Category>()
            .AnyAsync(c => c.Name == name);
        }

        public async Task<string> GetCategoryNameByID(int id)
        {
            return await _context.Categories.Where(c => c.Id == id)
                .Select(c => c.Name)
                .SingleOrDefaultAsync();
        }

    }
}
