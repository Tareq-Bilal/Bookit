using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Constants
{
    public static class TransactionType
    {
        public enum enTransactionType
        {
            Fee,       // Covers all charges (membership, processing, etc.)
            Fine,      // Specifically for overdue books
            Damage,    // For damaged books
            Refund     // For any refunded transaction
        }
        public static List<string> TransactionTypes { get; } = new List<string> { "Fee" ,"Fine" ,"Damage" ,"Refund" }; 
        public static enTransactionType Type { get; }
    }
}
