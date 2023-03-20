using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Origin = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Departure = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Arrival = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flight_FlightStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FlightStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FlightStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("701ed6a9-e40b-479f-af89-82f2234bc62a"), "Без задержек" });

            migrationBuilder.InsertData(
                table: "FlightStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("711ed6a9-e40b-479f-af89-82f2234bc62a"), "Задержка" });

            migrationBuilder.InsertData(
                table: "FlightStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("721ed6a9-e40b-479f-af89-82f2234bc62a"), "Отменён" });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Destination",
                table: "Flight",
                column: "Destination");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Origin_Destination",
                table: "Flight",
                columns: new[] { "Origin", "Destination" });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_StatusId",
                table: "Flight",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightStatus_Name",
                table: "FlightStatus",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "FlightStatus");
        }
    }
}
