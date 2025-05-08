using RepositoryPatternWithUOW.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Loan
{
    public class LoanAddDTO
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
        public DateTime? ReturnDate { get; set; } = null;
        public string Status { get; set; } = LoanStatus.enStatus.Active.ToString();

    }
}
