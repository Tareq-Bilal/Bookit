using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property (EF will ignore this in DB creation)
        public ICollection<Book>? Books { get; set; }

    }
}
