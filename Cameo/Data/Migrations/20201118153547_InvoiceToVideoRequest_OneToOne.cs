using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class InvoiceToVideoRequest_OneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceID",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoRequestID",
                table: "Invoices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_VideoRequestID",
                table: "Invoices",
                column: "VideoRequestID",
                unique: true,
                filter: "[VideoRequestID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_VideoRequests_VideoRequestID",
                table: "Invoices",
                column: "VideoRequestID",
                principalTable: "VideoRequests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_VideoRequests_VideoRequestID",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_VideoRequestID",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "VideoRequestID",
                table: "Invoices");
        }
    }
}
