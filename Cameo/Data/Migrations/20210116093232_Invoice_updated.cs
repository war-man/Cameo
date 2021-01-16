using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class Invoice_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transaction_id",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transaction_time",
                table: "Invoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "transaction_time",
                table: "Invoices");
        }
    }
}
