using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Constants
{

     public static class BookCopyStatus
    {
        public enum enStatus { Available, Loaned, Lost }
        public static List<string> Statuses { get; } = new List<string> { "Available", "Loaned", "Lost" };
        public static enStatus Status { get; }
    }
}
