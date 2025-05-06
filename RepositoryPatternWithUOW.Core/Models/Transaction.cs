using RepositoryPatternWithUOW.Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? LoanId { get; set; }  // Optional - for transactions related to loans
        public decimal Amount { get; set; }
        public int? BookId { get; set; }  // make it nullable if not always required

        public DateTime TransactionDate { get; set; }

        [MaxLength(30)]
        public string TransactionType { get; set; }
        
        [MaxLength(250)]
        public string? Description { get; set; }  // Can include specifics about the transaction

        // Navigation properties
        public User User { get; set; }
        public Loan Loan { get; set; }
        public Book Book { get; set; }

    }

}
