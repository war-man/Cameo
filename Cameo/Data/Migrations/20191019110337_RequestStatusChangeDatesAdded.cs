using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class RequestStatusChangeDatesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequestAccepted",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequestCanceledByCustomer",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequestCanceledByTalent",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequestExpired",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVideoCanceledByCustomer",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVideoCanceledByTalent",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVideoCompleted",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVideoExpired",
                table: "VideoRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRequestAccepted",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateRequestCanceledByCustomer",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateRequestCanceledByTalent",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateRequestExpired",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateVideoCanceledByCustomer",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateVideoCanceledByTalent",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateVideoCompleted",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DateVideoExpired",
                table: "VideoRequests");
        }
    }
}
