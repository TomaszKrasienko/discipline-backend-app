using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.application.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initial_activity_rule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    SelectedDays = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityRules_Title",
                table: "ActivityRules",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityRules");
        }
    }
}
