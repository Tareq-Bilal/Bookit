using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class BookWithAddedDate
    {
        public Book Book { get; set; }
        public DateTime AddedDate { get; set; }
    }

}
