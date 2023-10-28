using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class Student : ApplicationUser
    {
        //public string Id { get; set; }
        public string? CurrentLevel { get; set; }
        public decimal? PayedAmount { get; set; }
        public int? GroupId { get; set; }// Foreign Key
        public Group? Group { get; set; }
    }
}
