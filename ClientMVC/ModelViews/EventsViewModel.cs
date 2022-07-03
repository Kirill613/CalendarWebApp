using ClientMvc.Models;

namespace ClientMvc.ModelViews
{
    public class EventsViewModel
    {
        public Guid Id { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public DateTime prevDate { get; set; } = DateTime.Now;
        public Dictionary<DateOnly, List<EventDto>> AllEvents { get; set; }
        public Dictionary<DateOnly, List<EventDto>> Events5Days { get; set; }
        public Dictionary<EventDto, int> Forecast5Days { get; set; }
    }
}
