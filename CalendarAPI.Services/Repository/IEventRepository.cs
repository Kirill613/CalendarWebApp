namespace CalendarAPI.Services.Repository
{
    public interface IEventRepository
    {
        public Event ? GetEventByID(Guid eventtId);
        public IEnumerable<Event> GetEvents();
        public bool AddEvent(Event currEvent);
        public bool UpdateEvent(Event currEvent);
        public bool DeleteEvent(Guid eventtId);
        public void Save();
    }
}
