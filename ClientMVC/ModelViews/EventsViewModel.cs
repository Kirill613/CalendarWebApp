using ClientMvc.Models;

namespace ClientMvc.ModelViews
{
    public class EventsViewModel
    {
        public Guid Id { get; set; }
        public DateTime date { get; set; } = DateTime.Now;

        public List<EventDto> AllEvents { get; set; }
        public Dictionary<DateOnly, List<EventDto>> AllElementsByDays { get; set; }
        public Dictionary<EventDto, int> AllForecast5Days { get; set; }
    }
}
