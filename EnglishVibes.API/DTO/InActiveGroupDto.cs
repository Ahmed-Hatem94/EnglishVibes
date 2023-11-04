using EnglishVibes.Data.Models;

namespace EnglishVibes.API.DTO
{
    public class InActiveGroupDto
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string StudyPlan { get; set; } // private or group
        public bool ActiveStatus { get; set; }
        public ICollection<Student>? Students { get; set; } // Navigational property [Many]
    }
}
