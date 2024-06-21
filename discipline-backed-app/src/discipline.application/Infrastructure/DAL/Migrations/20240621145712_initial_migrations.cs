using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.application.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initial_migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    SelectedDays = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyProductivity",
                columns: table => new
                {
                    Day = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProductivity", x => x.Day);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsChecked = table.Column<bool>(type: "boolean", maxLength: 100, nullable: false),
                    ParentRuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    DailyProductivityDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_DailyProductivity_DailyProductivityDay",
                        column: x => x.DailyProductivityDay,
                        principalTable: "DailyProductivity",
                        principalColumn: "Day");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DailyProductivityDay",
                table: "Activities",
                column: "DailyProductivityDay");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityRules_Title",
                table: "ActivityRules",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductivity_Day",
                table: "DailyProductivity",
                column: "Day",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ActivityRules");

            migrationBuilder.DropTable(
                name: "DailyProductivity");
        }
    }
}
