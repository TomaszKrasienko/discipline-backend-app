using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.application.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initial_daily_productivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyProductivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Day = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProductivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsChecked = table.Column<bool>(type: "boolean", nullable: false),
                    ParentRuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    DailyProductivityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_DailyProductivity_DailyProductivityId",
                        column: x => x.DailyProductivityId,
                        principalTable: "DailyProductivities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DailyProductivityId",
                table: "Activities",
                column: "DailyProductivityId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductivity_Day",
                table: "DailyProductivities",
                column: "Day",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "DailyProductivities");
        }
    }
}
