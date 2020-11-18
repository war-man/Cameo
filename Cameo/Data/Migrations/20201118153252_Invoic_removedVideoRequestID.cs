using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class Invoic_removedVideoRequestID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_VideoRequests_VideoRequestID",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_VideoRequestID",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "VideoRequestID",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoRequestID",
                table: "Invoices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_VideoRequestID",
                table: "Invoices",
                column: "VideoRequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_VideoRequests_VideoRequestID",
                table: "Invoices",
                column: "VideoRequestID",
                principalTable: "VideoRequests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
