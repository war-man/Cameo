using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestNewDatesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCanceledByCustomer",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCanceledByTalent",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaymentConfirmed",
                table: "VideoRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCanceledByCustomer",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateCanceledByTalent",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DatePaymentConfirmed",
                table: "VideoRequests");
        }
    }
}
