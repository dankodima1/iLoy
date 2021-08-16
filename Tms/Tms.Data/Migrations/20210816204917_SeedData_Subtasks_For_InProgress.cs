using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tms.Data.Migrations
{
    public partial class SeedData_Subtasks_For_InProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Test subtask has Id = 007, parent Id = 006");

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Test subtask has Id = 008, parent Id = 006");

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Test subtask has Id = 009, parent Id = 006");

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Test subtask has Id = 010, parent Id = 006");

            migrationBuilder.InsertData(
                table: "TaskItem",
                columns: new[] { "Id", "Description", "FinishDateUtc", "Name", "ParentId", "StartDateUtc", "State" },
                values: new object[,]
                {
                    { 22, "Test subtask has Id = 022, parent Id = 013", new DateTime(2021, 9, 11, 15, 0, 0, 0, DateTimeKind.Utc), "Subtask 022", 13, new DateTime(2021, 9, 11, 14, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 23, "Test subtask has Id = 023, parent Id = 013", new DateTime(2021, 9, 12, 16, 0, 0, 0, DateTimeKind.Utc), "Subtask 023", 13, new DateTime(2021, 9, 12, 15, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 24, "Test subtask has Id = 024, parent Id = 016", new DateTime(2021, 9, 10, 17, 0, 0, 0, DateTimeKind.Utc), "Subtask 024", 16, new DateTime(2021, 9, 10, 16, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 25, "Test subtask has Id = 025, parent Id = 016", new DateTime(2021, 9, 11, 18, 0, 0, 0, DateTimeKind.Utc), "Subtask 025", 16, new DateTime(2021, 9, 11, 17, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 26, "Test subtask has Id = 026, parent Id = 016", new DateTime(2021, 9, 12, 19, 0, 0, 0, DateTimeKind.Utc), "Subtask 026", 16, new DateTime(2021, 9, 12, 18, 0, 0, 0, DateTimeKind.Utc), 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Test subtask has Id = 007, parent Id = 007");

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Test subtask has Id = 008, parent Id = 008");

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Test subtask has Id = 009, parent Id = 009");

            migrationBuilder.UpdateData(
                table: "TaskItem",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Test subtask has Id = 010, parent Id = 010");
        }
    }
}
