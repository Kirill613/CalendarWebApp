namespace CalendarAPI.Services.Repository
{
    public interface IEventRepository
    {
        public Task<Event?> GetEventByIDAsync(string eventtId);
        public Task<IEnumerable<Event>> GetEventsAsync(Guid id);
        public bool AddEvent(Event currEvent);
        public bool UpdateEvent(Event currEvent);
        public bool DeleteEvent(string eventtId);
        public void Save();
    }
}
