using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Linq;
using CalendarAPI.Services.Models;

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
                    Id = new Guid(),
                    Title = "Первое событие",
                    Message = "Пойти за цветами",

                    Date = new DateOnly(2021, 12, 21),
                    IsAllDayEvent = false,
                    BeginTime = new TimeOnly(20, 34, 00),
                    EndTime = new TimeOnly(21, 34, 00),

                    Lattitude = 23.232442,
                    Longtitude = 245.232442,
                    LocationName = "Park Aveny",

                    UserId = new Guid(),
                },
                new Event
                {
                    Id = new Guid(),
                    Title = "Второе событие",
                    Message = "Пойти в магазин",

                    Date = new DateOnly(2021, 12, 21),
                    IsAllDayEvent = false,
                    BeginTime = new TimeOnly(19, 34, 00),
                    EndTime = new TimeOnly(20, 34, 00),

                    Lattitude = 23.256442,
                    Longtitude = 245.256442,
                    LocationName = "Minsk, 34",

                    UserId = new Guid(),
                },
                new Event
                {
                    Id = new Guid(),
                    Title = "Третье событие",
                    Message = "Забрать посылку",

                    Date = new DateOnly(2021, 12, 21),
                    IsAllDayEvent = false,
                    BeginTime = new TimeOnly(10, 00, 00),
                    EndTime = new TimeOnly(12, 00, 00),

                    UserId = new Guid(),
                }
            );
        }

    }
}
