
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.DTO_s.Category
{
    public class CategoryGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property (EF will ignore this in DB creation)
    }
}
