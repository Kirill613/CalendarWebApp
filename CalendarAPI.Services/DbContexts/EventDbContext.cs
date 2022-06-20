namespace CalendarAPI.Services.DbContexts
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = new Guid("cc1916c8-9432-405e-875e-8f55b47018e2"),
                    Title = "Первое событие",
                    Message = "Пойти за цветами",

                    //Date = new DateOnly(2021, 12, 21),
                    IsAllDayEvent = false,
                    BeginTime = new DateTime(2021, 12, 21, 20, 34, 00),
                    EndTime = new DateTime(2021, 12, 21, 21, 34, 00),

                    Lattitude = 23.232442,
                    Longtitude = 245.232442,
                    LocationName = "Park Aveny",

                    UserId = new Guid(),
                },
                new Event
                {
                    Id = new Guid("d51dc8ac-b93e-459e-b9e1-71890d015255"),
                    Title = "Второе событие",
                    Message = "Пойти в магазин",

                    //Date = new DateOnly(2021, 12, 21),
                    IsAllDayEvent = false,
                    BeginTime = new DateTime(2021, 12, 21, 19, 34, 00),
                    EndTime = new DateTime(2021, 12, 21, 20, 34, 00),

                    Lattitude = 23.256442,
                    Longtitude = 245.256442,
                    LocationName = "Minsk, 34",

                    UserId = new Guid(),
                },
                new Event
                {
                    Id = new Guid("f72cb825-3f91-49df-83e7-ded4be310ac0"),
                    Title = "Третье событие",
                    Message = "Забрать посылку",

                    //Date = new DateOnly(2021, 12, 21),
                    IsAllDayEvent = false,
                    BeginTime = new DateTime(2021, 12, 21, 10, 00, 00),
                    EndTime = new DateTime(2021, 12, 21, 12, 00, 00),

                    UserId = new Guid(),
                }
            );
        }

    }
}
