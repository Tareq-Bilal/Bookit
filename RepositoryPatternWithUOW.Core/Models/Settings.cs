using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public decimal Value { get; set; } // store all values as strings

    }

}
