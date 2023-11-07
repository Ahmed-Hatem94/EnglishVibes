namespace EnglishVibes.API.DTO
{
    public class CompleteGroupDataDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public Guid InstructorId { get; set; }
        public int Day1 { get; set; }
        public int Day2 { get; set; }
    }
}
