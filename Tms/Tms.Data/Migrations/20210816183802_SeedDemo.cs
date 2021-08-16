using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tms.Data.Migrations
{
    public partial class SeedDemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaskItem",
                columns: new[] { "Id", "Description", "FinishDateUtc", "Name", "ParentId", "StartDateUtc", "State" },
                values: new object[,]
                {
                    { 1, "Test task has Id = 001", new DateTime(2021, 9, 2, 11, 0, 0, 0, DateTimeKind.Utc), "Task 001", null, new DateTime(2021, 9, 2, 10, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 2, "Test task has Id = 002", new DateTime(2021, 9, 3, 11, 0, 0, 0, DateTimeKind.Utc), "Task 002", null, new DateTime(2021, 9, 3, 10, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 3, "Test task has Id = 003", new DateTime(2021, 9, 4, 11, 0, 0, 0, DateTimeKind.Utc), "Task 003", null, new DateTime(2021, 9, 4, 10, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 4, "Test task has Id = 004", new DateTime(2021, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), "Task 004", null, new DateTime(2021, 9, 5, 10, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 5, "Test task has Id = 005", new DateTime(2021, 9, 6, 11, 0, 0, 0, DateTimeKind.Utc), "Task 005", null, new DateTime(2021, 9, 6, 10, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 6, "Test task has Id = 006", new DateTime(2021, 9, 7, 22, 0, 0, 0, DateTimeKind.Utc), "Task 006 (parent)", null, new DateTime(2021, 9, 7, 21, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 11, "Test task has Id = 011", new DateTime(2021, 9, 1, 11, 0, 0, 0, DateTimeKind.Utc), "Task 011 (InProgress)", null, new DateTime(2021, 9, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 12, "Test task has Id = 012", new DateTime(2021, 9, 2, 11, 0, 0, 0, DateTimeKind.Utc), "Task 012 (InProgress)", null, new DateTime(2021, 9, 2, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 13, "Test task has Id = 013", new DateTime(2021, 9, 3, 11, 0, 0, 0, DateTimeKind.Utc), "Task 013 (InProgress)", null, new DateTime(2021, 9, 3, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 14, "Test task has Id = 014", new DateTime(2021, 9, 4, 11, 0, 0, 0, DateTimeKind.Utc), "Task 014 (InProgress)", null, new DateTime(2021, 9, 4, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 15, "Test task has Id = 015", new DateTime(2021, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), "Task 015 (InProgress)", null, new DateTime(2021, 9, 5, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 16, "Test task has Id = 016", new DateTime(2021, 9, 6, 11, 0, 0, 0, DateTimeKind.Utc), "Task 016 (InProgress)", null, new DateTime(2021, 9, 6, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 17, "Test task has Id = 017", new DateTime(2021, 9, 7, 11, 0, 0, 0, DateTimeKind.Utc), "Task 017 (InProgress)", null, new DateTime(2021, 9, 7, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 18, "Test task has Id = 018", new DateTime(2021, 9, 8, 11, 0, 0, 0, DateTimeKind.Utc), "Task 018 (InProgress)", null, new DateTime(2021, 9, 8, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 19, "Test task has Id = 019", new DateTime(2021, 9, 9, 11, 0, 0, 0, DateTimeKind.Utc), "Task 019 (InProgress)", null, new DateTime(2021, 9, 9, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 20, "Test task has Id = 020", new DateTime(2021, 9, 10, 11, 0, 0, 0, DateTimeKind.Utc), "Task 020 (InProgress)", null, new DateTime(2021, 9, 10, 10, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.InsertData(
                table: "TaskItem",
                columns: new[] { "Id", "Description", "FinishDateUtc", "Name", "ParentId", "StartDateUtc", "State" },
                values: new object[,]
                {
                    { 7, "Test subtask has Id = 007, parent Id = 007", new DateTime(2021, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Subtask 007", 6, new DateTime(2021, 9, 2, 23, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 8, "Test subtask has Id = 008, parent Id = 008", new DateTime(2021, 9, 4, 1, 0, 0, 0, DateTimeKind.Utc), "Subtask 008", 6, new DateTime(2021, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 9, "Test subtask has Id = 009, parent Id = 009", new DateTime(2021, 9, 5, 2, 0, 0, 0, DateTimeKind.Utc), "Subtask 009", 6, new DateTime(2021, 9, 5, 1, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 10, "Test subtask has Id = 010, parent Id = 010", new DateTime(2021, 9, 6, 3, 0, 0, 0, DateTimeKind.Utc), "Subtask 010", 6, new DateTime(2021, 9, 6, 2, 0, 0, 0, DateTimeKind.Utc), 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
