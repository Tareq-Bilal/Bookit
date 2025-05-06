namespace RepositoryPatternwithUOW.Api.DTO_s.Transaction
{
    public class TransactionGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? LoanId { get; set; }
        public string BookTitle { get; set; } // Only populated when LoanId is not null
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } // Fee, Fine, Damage, Refund
        public string? Description { get; set; }
    }
}
