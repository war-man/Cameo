using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class RequestStatusIDRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_VideoRequestStatuses_RequestStatusID",
                table: "VideoRequests");

            migrationBuilder.AlterColumn<int>(
                name: "RequestStatusID",
                table: "VideoRequests",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerJobID",
                table: "VideoRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_VideoRequestStatuses_RequestStatusID",
                table: "VideoRequests",
                column: "RequestStatusID",
                principalTable: "VideoRequestStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_VideoRequestStatuses_RequestStatusID",
                table: "VideoRequests");

            migrationBuilder.AlterColumn<int>(
                name: "RequestStatusID",
                table: "VideoRequests",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "AnswerJobID",
                table: "VideoRequests",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_VideoRequestStatuses_RequestStatusID",
                table: "VideoRequests",
                column: "RequestStatusID",
                principalTable: "VideoRequestStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
