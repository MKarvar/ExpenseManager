using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManager.Infrastructure.Migrations
{
    public partial class change_field_name2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseTakeParters");

            migrationBuilder.CreateTable(
                name: "ExpensePartTakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartTakerId = table.Column<int>(type: "int", nullable: false),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    ShareAmount = table.Column<double>(type: "float", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SettleDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePartTakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePartTakers_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePartTakers_Users_PartTakerId",
                        column: x => x.PartTakerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 3, 17, 46, 754, DateTimeKind.Local).AddTicks(7425), new Guid("4a454c37-99ae-43ed-9cd0-59e919d45e7a") });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePartTakers_ExpenseId",
                table: "ExpensePartTakers",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePartTakers_PartTakerId",
                table: "ExpensePartTakers",
                column: "PartTakerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensePartTakers");

            migrationBuilder.CreateTable(
                name: "ExpenseTakeParters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SettleDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShareAmount = table.Column<double>(type: "float", nullable: false),
                    TakeParterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTakeParters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseTakeParters_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseTakeParters_Users_TakeParterId",
                        column: x => x.TakeParterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RegistrationDateTime", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 7, 27, 2, 23, 50, 816, DateTimeKind.Local).AddTicks(9810), new Guid("b94cb8c0-352c-4430-9d6e-b0070377f3da") });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTakeParters_ExpenseId",
                table: "ExpenseTakeParters",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTakeParters_TakeParterId",
                table: "ExpenseTakeParters",
                column: "TakeParterId");
        }
    }
}
