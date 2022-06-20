using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarAPI.Services.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAllDayEvent = table.Column<bool>(type: "bit", nullable: false),
                    BeginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lattitude = table.Column<double>(type: "float", nullable: true),
                    Longtitude = table.Column<double>(type: "float", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BeginTime", "EndTime", "IsAllDayEvent", "Lattitude", "LocationName", "Longtitude", "Message", "Title", "UserId" },
                values: new object[] { new Guid("cc1916c8-9432-405e-875e-8f55b47018e2"), new DateTime(2021, 12, 21, 20, 34, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 12, 21, 21, 34, 0, 0, DateTimeKind.Unspecified), false, 23.232441999999999, "Park Aveny", 245.23244199999999, "Пойти за цветами", "Первое событие", new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BeginTime", "EndTime", "IsAllDayEvent", "Lattitude", "LocationName", "Longtitude", "Message", "Title", "UserId" },
                values: new object[] { new Guid("d51dc8ac-b93e-459e-b9e1-71890d015255"), new DateTime(2021, 12, 21, 19, 34, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 12, 21, 20, 34, 0, 0, DateTimeKind.Unspecified), false, 23.256442, "Minsk, 34", 245.25644199999999, "Пойти в магазин", "Второе событие", new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BeginTime", "EndTime", "IsAllDayEvent", "Lattitude", "LocationName", "Longtitude", "Message", "Title", "UserId" },
                values: new object[] { new Guid("f72cb825-3f91-49df-83e7-ded4be310ac0"), new DateTime(2021, 12, 21, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 12, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, "Забрать посылку", "Третье событие", new Guid("00000000-0000-0000-0000-000000000000") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
