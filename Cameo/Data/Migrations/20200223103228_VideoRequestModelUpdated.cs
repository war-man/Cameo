using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountToBeTakenOffBalance",
                table: "VideoRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "WebsiteCommission",
                table: "VideoRequests",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountToBeTakenOffBalance",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "WebsiteCommission",
                table: "VideoRequests");
        }
    }
}
