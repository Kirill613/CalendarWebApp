namespace ClientMvc.Models
{
    public class EventDto
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }
        public string? Message { get; set; }

        public bool IsAllDayEvent { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public double? Lattitude { get; set; }
        public double? Longtitude { get; set; }
        public string? LocationName { get; set; }

        public Guid UserId { get; set; }
    }
}
