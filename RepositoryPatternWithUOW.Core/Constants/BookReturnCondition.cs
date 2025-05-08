using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Constants
{
    
    public static class BookReturnCondition
    {

        public enum enCondition { Good, Damaged }
        public enum enSetting { LateReturnFeePerDay }
        public static List<string> Conditions { get; } = new List<string> { "Good" , "Damaged"};
        public static List<string> Settings { get; } = new List<string> { "LateReturnFeePerDay" };

    }
}
