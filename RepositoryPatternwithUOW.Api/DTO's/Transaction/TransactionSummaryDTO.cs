namespace RepositoryPatternwithUOW.Api.DTO_s.Transaction
{
    public class TransactionSummaryDto
    {
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
        public Dictionary<string, int> CountByType { get; set; }
        public Dictionary<string, decimal> AmountByType { get; set; }
    
    }
}
