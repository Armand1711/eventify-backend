using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToEventTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "assignedTo",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "processedByUserId",
                table: "EventRequests");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "EventTasks",
                newName: "EventTasks",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Events",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "EventRequests",
                newName: "EventRequests",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "Budgets",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Archives",
                newName: "Archives",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                schema: "public",
                table: "EventRequests",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "title",
                schema: "public",
                table: "EventRequests",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "public",
                table: "EventRequests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "requesterName",
                schema: "public",
                table: "EventRequests",
                newName: "RequesterName");

            migrationBuilder.RenameColumn(
                name: "requesterEmail",
                schema: "public",
                table: "EventRequests",
                newName: "RequesterEmail");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "public",
                table: "EventRequests",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "date",
                schema: "public",
                table: "EventRequests",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                schema: "public",
                table: "EventRequests",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                schema: "public",
                table: "EventTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "public",
                table: "EventRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventTasks_eventId",
                schema: "public",
                table: "EventTasks",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTasks_userId",
                schema: "public",
                table: "EventTasks",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_userId",
                schema: "public",
                table: "Events",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_userId",
                schema: "public",
                table: "Events",
                column: "userId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTasks_Events_eventId",
                schema: "public",
                table: "EventTasks",
                column: "eventId",
                principalSchema: "public",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTasks_Users_userId",
                schema: "public",
                table: "EventTasks",
                column: "userId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_userId",
                schema: "public",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTasks_Events_eventId",
                schema: "public",
                table: "EventTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTasks_Users_userId",
                schema: "public",
                table: "EventTasks");

            migrationBuilder.DropIndex(
                name: "IX_EventTasks_eventId",
                schema: "public",
                table: "EventTasks");

            migrationBuilder.DropIndex(
                name: "IX_EventTasks_userId",
                schema: "public",
                table: "EventTasks");

            migrationBuilder.DropIndex(
                name: "IX_Events_userId",
                schema: "public",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "userId",
                schema: "public",
                table: "EventTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "EventRequests");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "public",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "EventTasks",
                schema: "public",
                newName: "EventTasks");

            migrationBuilder.RenameTable(
                name: "Events",
                schema: "public",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "EventRequests",
                schema: "public",
                newName: "EventRequests");

            migrationBuilder.RenameTable(
                name: "Budgets",
                schema: "public",
                newName: "Budgets");

            migrationBuilder.RenameTable(
                name: "Archives",
                schema: "public",
                newName: "Archives");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "EventRequests",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "EventRequests",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "EventRequests",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "RequesterName",
                table: "EventRequests",
                newName: "requesterName");

            migrationBuilder.RenameColumn(
                name: "RequesterEmail",
                table: "EventRequests",
                newName: "requesterEmail");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EventRequests",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "EventRequests",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EventRequests",
                newName: "createdAt");

            migrationBuilder.AddColumn<string>(
                name: "assignedTo",
                table: "EventTasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "processedByUserId",
                table: "EventRequests",
                type: "integer",
                nullable: true);
        }
    }
}
