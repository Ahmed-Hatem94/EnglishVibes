using EnglishVibes.Data.Models;

namespace EnglishVibes.API.DTO
{
    public class ActiveGroupDto
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public DateTime? TimeSlot { get; set; }
        public Guid? InstructorId { get; set; }// Foreign Key
        public Instructor? Instructor { get; set; }  // Navigational property [One]
        public ICollection<Student>? Students { get; set; } // Navigational property [Many]
    }
}
