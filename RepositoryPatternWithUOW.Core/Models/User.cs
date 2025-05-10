using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Email { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<Loan> Loans { get; set; }
        //public ICollection<Review> Reviews { get; set; }  
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Wishlist> WishlistItems { get; set; }
    }
}
