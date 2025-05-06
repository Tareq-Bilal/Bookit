using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } // Active, Returned, Overdue

        // Navigation properties
        public Book Book { get; set; }
        public User User { get; set; }
        public BookCopy BookCopy { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

    }
}
