using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideRequestModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentJobID",
                table: "VideoRequests",
                newName: "PaymentConfirmationJobID");

            migrationBuilder.RenameColumn(
                name: "PaymentDeadline",
                table: "VideoRequests",
                newName: "PaymentConfirmationDeadline");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaymentConfirmationExpired",
                table: "VideoRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePaymentConfirmationExpired",
                table: "VideoRequests");

            migrationBuilder.RenameColumn(
                name: "PaymentConfirmationJobID",
                table: "VideoRequests",
                newName: "PaymentJobID");

            migrationBuilder.RenameColumn(
                name: "PaymentConfirmationDeadline",
                table: "VideoRequests",
                newName: "PaymentDeadline");
        }
    }
}
