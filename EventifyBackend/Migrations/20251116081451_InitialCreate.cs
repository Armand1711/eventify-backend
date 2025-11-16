using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_userId",
                schema: "public",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "EventRequests",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "EventRequests",
                newName: "updatedat");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "public",
                table: "EventRequests",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "EventRequests",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "public",
                table: "EventRequests",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "public",
                table: "EventRequests",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "EventRequests",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "RequesterName",
                schema: "public",
                table: "EventRequests",
                newName: "requester_name");

            migrationBuilder.RenameColumn(
                name: "RequesterEmail",
                schema: "public",
                table: "EventRequests",
                newName: "requester_email");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                schema: "public",
                table: "EventTasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "priority",
                schema: "public",
                table: "EventTasks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "public",
                table: "EventTasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "budget",
                schema: "public",
                table: "EventTasks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "archived",
                schema: "public",
                table: "EventTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                schema: "public",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "public",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "archived",
                schema: "public",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                schema: "public",
                table: "EventRequests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedat",
                schema: "public",
                table: "EventRequests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdat",
                schema: "public",
                table: "EventRequests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                schema: "public",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                schema: "public",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "public",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_EventRequests_userId",
                schema: "public",
                table: "EventRequests",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_eventId",
                schema: "public",
                table: "Archives",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_userId",
                schema: "public",
                table: "Archives",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Archives_Events_eventId",
                schema: "public",
                table: "Archives",
                column: "eventId",
                principalSchema: "public",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Archives_Users_userId",
                schema: "public",
                table: "Archives",
                column: "userId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventRequests_Users_userId",
                schema: "public",
                table: "EventRequests",
                column: "userId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_userId",
                schema: "public",
                table: "Events",
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
                name: "FK_Archives_Events_eventId",
                schema: "public",
                table: "Archives");

            migrationBuilder.DropForeignKey(
                name: "FK_Archives_Users_userId",
                schema: "public",
                table: "Archives");

            migrationBuilder.DropForeignKey(
                name: "FK_EventRequests_Users_userId",
                schema: "public",
                table: "EventRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_userId",
                schema: "public",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EventRequests_userId",
                schema: "public",
                table: "EventRequests");

            migrationBuilder.DropIndex(
                name: "IX_Archives_eventId",
                schema: "public",
                table: "Archives");

            migrationBuilder.DropIndex(
                name: "IX_Archives_userId",
                schema: "public",
                table: "Archives");

            migrationBuilder.RenameColumn(
                name: "userId",
                schema: "public",
                table: "EventRequests",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updatedat",
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
                name: "createdat",
                schema: "public",
                table: "EventRequests",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "requester_name",
                schema: "public",
                table: "EventRequests",
                newName: "RequesterName");

            migrationBuilder.RenameColumn(
                name: "requester_email",
                schema: "public",
                table: "EventRequests",
                newName: "RequesterEmail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                schema: "public",
                table: "EventTasks",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "priority",
                schema: "public",
                table: "EventTasks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "public",
                table: "EventTasks",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "budget",
                schema: "public",
                table: "EventTasks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "archived",
                schema: "public",
                table: "EventTasks",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                schema: "public",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "public",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<bool>(
                name: "archived",
                schema: "public",
                table: "Events",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "public",
                table: "EventRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "public",
                table: "EventRequests",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "public",
                table: "EventRequests",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                schema: "public",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                schema: "public",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "public",
                table: "Archives",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_userId",
                schema: "public",
                table: "Events",
                column: "userId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
