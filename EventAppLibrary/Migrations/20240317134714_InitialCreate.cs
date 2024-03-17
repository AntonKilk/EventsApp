using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAppLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventPlace = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyRegistrationCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonalCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_EventId",
                table: "Companies",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_EventId",
                table: "Persons",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
