using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Constants
{
    public static class LoanStatus
    {
        public static List<string> Status { get; } = new List<string> { "Active", "Returned", "Overdue" };
    }
}
