using EnglishVibes.Data.Models;

namespace EnglishVibes.API.DTO
{
    public class ActiveGroupDto
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TimeSlot { get; set; }
        public List<string> GroupWeekDays { get; set; } = new List<string>();
        public Guid? InstructorId { get; set; }
        public string Instructor { get; set; }
        public List<string> Students { get; set; } = new List<string>();
    }
}
