using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideRequestModelAddedNewFieldDatePaymentScreenshotUploaded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaymentScreenshotUploaded",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentScreenshotID",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_PaymentScreenshotID",
                table: "VideoRequests",
                column: "PaymentScreenshotID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_Attachments_PaymentScreenshotID",
                table: "VideoRequests",
                column: "PaymentScreenshotID",
                principalTable: "Attachments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_Attachments_PaymentScreenshotID",
                table: "VideoRequests");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_PaymentScreenshotID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "DatePaymentScreenshotUploaded",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "PaymentScreenshotID",
                table: "VideoRequests");
        }
    }
}
