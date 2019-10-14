using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestPartialCreateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Customers_CustomerID",
                table: "VideoRequests");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_CustomerID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "VideoRequests");

            migrationBuilder.AddColumn<int>(
                name: "TalentID",
                table: "VideoRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_CustomertID",
                table: "VideoRequests",
                column: "CustomertID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_TalentID",
                table: "VideoRequests",
                column: "TalentID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Customers_CustomertID",
                table: "VideoRequests",
                column: "CustomertID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Talents_TalentID",
                table: "VideoRequests",
                column: "TalentID",
                principalTable: "Talents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Customers_CustomertID",
                table: "VideoRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Talents_TalentID",
                table: "VideoRequests");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_CustomertID",
                table: "VideoRequests");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_TalentID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "TalentID",
                table: "VideoRequests");

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_CustomerID",
                table: "VideoRequests",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Customers_CustomerID",
                table: "VideoRequests",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
