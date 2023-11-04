using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class Student : ApplicationUser
    {
        //public string Id { get; set; }
        public string? CurrentLevel { get; set; }
        public decimal? PayedAmount { get; set; }
        public bool ActiveStatus { get; set; }
        public string? StudyPlan { get; set; }   // Private Or Group
        public int? GroupId { get; set; }        // Foreign Key
      
        public Group? Group { get; set; }
    }
}
