using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Constants
{
    public static class LoanStatus
    {
        public enum enStatus { Active , Returned , Overdue }
        public static List<string> Statuses { get; } = new List<string> { "Active", "Returned", "Overdue" };
        public static enStatus Status { get; }
    
    }
}
