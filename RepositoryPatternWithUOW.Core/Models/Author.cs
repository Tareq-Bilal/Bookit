using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF
{
    public class Author
    {
        public int Id { get; set; }

        [Required , MaxLength(150)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string? Biography { get; set; }    // Short biography or description
        public DateTime DateOfBirth { get; set; }
        

    }
}
