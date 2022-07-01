using ClientMvc.Models;

namespace ClientMvc.ModelViews
{
    public class EventsViewModel
    {
        public Guid Id { get; set; }
        public List<EventDto> AllEvents { get; set; }
    }
}
