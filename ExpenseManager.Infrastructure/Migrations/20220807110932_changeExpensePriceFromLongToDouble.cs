using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManager.Infrastructure.Migrations
{
    public partial class changeExpensePriceFromLongToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "Expenses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 8, 7, 15, 39, 31, 713, DateTimeKind.Local).AddTicks(624), new Guid("47d3580d-c2ab-4e32-a217-e13021e77da5") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TotalPrice",
                table: "Expenses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 4, 45, 6, 881, DateTimeKind.Local).AddTicks(2619), new Guid("17bd837a-9a58-425e-bf98-45d24fa13f5b") });
        }
    }
}
