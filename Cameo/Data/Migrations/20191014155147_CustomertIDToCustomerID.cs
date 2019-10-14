using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class CustomertIDToCustomerID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Customers_CustomertID",
                table: "VideoRequests");

            migrationBuilder.RenameColumn(
                name: "CustomertID",
                table: "VideoRequests",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_VideoRequests_CustomertID",
                table: "VideoRequests",
                newName: "IX_VideoRequests_CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Customers_CustomerID",
                table: "VideoRequests",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Customers_CustomerID",
                table: "VideoRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "VideoRequests",
                newName: "CustomertID");

            migrationBuilder.RenameIndex(
                name: "IX_VideoRequests_CustomerID",
                table: "VideoRequests",
                newName: "IX_VideoRequests_CustomertID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Customers_CustomertID",
                table: "VideoRequests",
                column: "CustomertID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
