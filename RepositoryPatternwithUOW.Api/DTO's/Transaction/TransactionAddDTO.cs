using Microsoft.VisualBasic;
using RepositoryPatternWithUOW.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Transaction
{
    public class TransactionAddDTO
    {
        [Required]
        public int UserId { get; set; }

        public int? LoanId { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Amount { get; set; } = 0;
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public string TransactionType { get; set; } = RepositoryPatternWithUOW.Core.Constants.TransactionType.TransactionTypes.First().ToString();
        public string? Description { get; set; } = string.Empty; // Can include specifics about the transaction

    }
}
