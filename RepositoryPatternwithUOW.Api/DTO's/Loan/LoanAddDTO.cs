using RepositoryPatternWithUOW.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Loan
{
    public class LoanAddDTO
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = LoanStatus.Statuses.First();

    }
}
