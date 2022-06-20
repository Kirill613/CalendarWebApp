namespace CalendarAPI.Services.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string ? Title { get; set; }
        public string ? Message { get; set; }

        public bool IsAllDayEvent { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public string ? LocationName { get; set; }


        public Guid? DayId { get; set; }
        public Guid UserId { get; set; }
    }
}
