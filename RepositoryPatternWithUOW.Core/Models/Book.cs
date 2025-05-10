using RepositoryPatternWithUOW.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required , MaxLength(150)]
        public string Title { get; set; }

        public DateOnly? PublicationDate { get; set; }
        public Author Author { get; set; } 

        public int AuthorID { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public ICollection<BookCopy> BookCopies { get; set; }
        public ICollection<Loan> Loans { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }


    }
}
