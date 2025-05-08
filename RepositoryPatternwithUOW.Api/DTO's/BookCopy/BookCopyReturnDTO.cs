namespace RepositoryPatternwithUOW.Api.DTO_s.BookCopy
{
    public class BookCopyReturnDTO
    {
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public string Condition { get; set; } // Optional - if tracking condition on return
        public string? Notes { get; set; }
        public decimal AdditionalCharges { get; set; } = 0;
    }
}
