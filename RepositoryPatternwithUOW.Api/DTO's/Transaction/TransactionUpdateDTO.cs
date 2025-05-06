using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Transaction
{
    public class TransactionUpdateDTO
    {
        [Range(0.01, 10000)] // Adjust range as needed
        public decimal Amount { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }
    }
}
