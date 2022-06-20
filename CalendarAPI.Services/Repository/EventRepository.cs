namespace CalendarAPI.Services.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _dbContext;

        public EventRepository(EventDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Event ? GetEventByID(Guid eventtId)
        {
            return _dbContext.Events.FirstOrDefault(ev => ev.Id == eventtId);
        }

        public IEnumerable<Event> GetEvents()
        {
            return _dbContext.Events.ToList();
        }

        public bool AddEvent(Event currEvent)
        {
            if (GetEventByID(currEvent.Id) != null)
                return false;

            _dbContext.Add(currEvent);
            Save();

            return true;
        }

        public bool UpdateEvent(Event currEvent)
        {
            var temp = GetEventByID(currEvent.Id);
            if (temp == null)
                return false;
            _dbContext.Entry(temp).State = EntityState.Detached;

            _dbContext.Update(currEvent);
            Save();

            return true;
        }

        public bool DeleteEvent(Guid eventId)
        {
            var currEvent = GetEventByID(eventId);

            if (currEvent == null)
                return false;

            _dbContext.Events.Remove(currEvent);
            Save();
            return true;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
