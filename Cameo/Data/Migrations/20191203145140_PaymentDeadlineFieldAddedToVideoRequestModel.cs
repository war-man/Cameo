using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class PaymentDeadlineFieldAddedToVideoRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaymentExpired",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDeadline",
                table: "VideoRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePaymentExpired",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "PaymentDeadline",
                table: "VideoRequests");
        }
    }
}
