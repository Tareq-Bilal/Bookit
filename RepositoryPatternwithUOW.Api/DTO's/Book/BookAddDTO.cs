using RepositoryPatternWithUOW.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.DTO_s.Book
{
    public class BookAddDTO
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public int CategoryId { get; set; }
        public DateOnly? PublicationDate { get; set; }


    }
}
