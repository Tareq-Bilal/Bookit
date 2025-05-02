using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Constants
{

     public static class BookCopyStatus
    {
        public static List<string> Status { get; } = new List<string> { "Available", "Loaned", "Lost" };
    }
}
