using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventifyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddEventRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userid",
                table: "Budgets",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "eventid",
                table: "Budgets",
                newName: "taskId");

            migrationBuilder.AddColumn<bool>(
                name: "archived",
                table: "EventTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "archived",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EventRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    requesterName = table.Column<string>(type: "text", nullable: false),
                    requesterEmail = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRequests");

            migrationBuilder.DropColumn(
                name: "archived",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "archived",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Budgets",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "taskId",
                table: "Budgets",
                newName: "eventid");
        }
    }
}
