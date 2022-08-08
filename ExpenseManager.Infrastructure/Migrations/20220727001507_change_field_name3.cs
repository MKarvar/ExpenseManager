using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManager.Infrastructure.Migrations
{
    public partial class change_field_name3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 4, 45, 6, 881, DateTimeKind.Local).AddTicks(2619), new Guid("17bd837a-9a58-425e-bf98-45d24fa13f5b") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 3, 17, 46, 754, DateTimeKind.Local).AddTicks(7425), new Guid("4a454c37-99ae-43ed-9cd0-59e919d45e7a") });
        }
    }
}
