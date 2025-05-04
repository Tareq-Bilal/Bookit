using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternwithUOW.Api.DTO_s.Loan
{
    public class LoanUpdateDTO
    {
        public  DateTime BorrowDate { get; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = LoanStatus.Statuses.First();
    
    }



}
