namespace CalendarAPI.Services.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _dbContext;

        public EventRepository(EventDbContext dbContext)
        {
            _dbContext = dbContext;
        }   
        public async Task<Event?> GetEventByIDAsync(string eventId)
        {
            try
            {
                Guid _eventId = new Guid(eventId);
                return await GetEventByIDAsync(_eventId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IQueryable<Event>> GetEventsAsync()
        {
            return _dbContext.Events;
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

        public bool DeleteEvent(string eventId)
        {
            var currEvent = GetEventByID(eventId);

            if (currEvent == null)
                return false;

            _dbContext.Remove(currEvent);
            Save();
            return true;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }



        private Event? GetEventByID(Guid eventId)
        {
            return _dbContext.Events.FirstOrDefault(ev => ev.Id == eventId);
        }
        private Event? GetEventByID(string eventId)
        {
            try
            {
                Guid _eventId = new Guid(eventId);
                return GetEventByID(_eventId);
            }
            catch
            {
                return null;
            }
        }
        private async Task<Event?> GetEventByIDAsync(Guid eventId)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(ev => ev.Id == eventId);
        }
    }
}
