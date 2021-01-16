using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class Invoice_addedPaymoResponseFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "hold_till",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<float>(
                name: "commission_value",
                table: "Invoices",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "confirm_time",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "duration_in_minutes",
                table: "Invoices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "prepay_time",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status_code",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status_message",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "terminal_id",
                table: "Invoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "commission_value",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "confirm_time",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "duration_in_minutes",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "prepay_time",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "status_code",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "status_message",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "terminal_id",
                table: "Invoices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "hold_till",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
