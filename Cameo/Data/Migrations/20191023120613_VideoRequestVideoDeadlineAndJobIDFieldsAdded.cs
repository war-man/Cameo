using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestVideoDeadlineAndJobIDFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerJobID",
                table: "VideoRequests",
                newName: "VideoJobID");

            migrationBuilder.RenameColumn(
                name: "AnswerDeadline",
                table: "VideoRequests",
                newName: "RequestAnswerDeadline");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaid",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestAnswerJobID",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VideoDeadline",
                table: "VideoRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePaid",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "RequestAnswerJobID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "VideoDeadline",
                table: "VideoRequests");

            migrationBuilder.RenameColumn(
                name: "VideoJobID",
                table: "VideoRequests",
                newName: "AnswerJobID");

            migrationBuilder.RenameColumn(
                name: "RequestAnswerDeadline",
                table: "VideoRequests",
                newName: "AnswerDeadline");
        }
    }
}
