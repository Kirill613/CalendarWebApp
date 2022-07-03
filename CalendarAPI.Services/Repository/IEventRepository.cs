namespace CalendarAPI.Services.Repository
{
    public interface IEventRepository
    {
        public Task<IQueryable<Event>> GetEventsAsync();
        public Task<Event?> GetEventByIDAsync(string eventId);
        public bool AddEvent(Event currEvent);
        public bool UpdateEvent(Event currEvent);
        public bool DeleteEvent(string eventId);
        public void Save();
    }
}
