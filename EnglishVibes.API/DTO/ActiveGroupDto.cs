using EnglishVibes.Data.Models;

namespace EnglishVibes.API.DTO
{
    public class ActiveGroupDto
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TimeSlot { get; set; }
        public ICollection<GroupWeekDays> GroupWeekDays { get; set; }
        public Guid? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
