using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyBackend.Migrations
{
    /// <inheritdoc />
    public partial class RecreateEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EventTasks");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "EventTasks",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventTasks",
                newName: "eventId");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "EventTasks",
                newName: "dueDate");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EventTasks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "EventTasks",
                newName: "completed");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Events",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Events",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Events",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Events",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Budgets",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Budgets",
                newName: "eventid");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Budgets",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Budgets",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Archives",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Archives",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Archives",
                newName: "eventId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Archives",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Archives",
                newName: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "assignedTo",
                table: "EventTasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "budget",
                table: "EventTasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "EventTasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "priority",
                table: "EventTasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "EventTasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "assignedTo",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "budget",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Archives");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Archives");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "EventTasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "eventId",
                table: "EventTasks",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "dueDate",
                table: "EventTasks",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "EventTasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "completed",
                table: "EventTasks",
                newName: "IsCompleted");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Events",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Events",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Events",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Events",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "Budgets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "eventid",
                table: "Budgets",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "Budgets",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Budgets",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Archives",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Archives",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "eventId",
                table: "Archives",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Archives",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Archives",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EventTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
