using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManager.Infrastructure.Migrations
{
    public partial class change_field_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 2, 23, 50, 816, DateTimeKind.Local).AddTicks(9810), new Guid("b94cb8c0-352c-4430-9d6e-b0070377f3da") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 1, 49, 26, 40, DateTimeKind.Local).AddTicks(827), new Guid("d83ea151-9d27-4803-a7e9-9b6ae9f29138") });
        }
    }
}
