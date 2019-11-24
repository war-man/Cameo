using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestModelAddedVideoField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoID",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_VideoID",
                table: "VideoRequests",
                column: "VideoID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Attachments_VideoID",
                table: "VideoRequests",
                column: "VideoID",
                principalTable: "Attachments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Attachments_VideoID",
                table: "VideoRequests");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_VideoID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "VideoID",
                table: "VideoRequests");
        }
    }
}
