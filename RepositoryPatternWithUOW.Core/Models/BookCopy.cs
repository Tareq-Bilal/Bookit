using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class BookCopy
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } // Available, Loaned, Lost
        public DateTime AcquisitionDate { get; set; }
        public Book Book { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
