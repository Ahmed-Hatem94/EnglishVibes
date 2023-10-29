using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public bool ActiveStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TimeSlot { get; set; }
        public Guid? InstructorId { get; set; }// Foreign Key
        public Instructor? Instructor { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
