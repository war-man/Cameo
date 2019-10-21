using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class RequestAnswerJobIDIsNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AnswerJobID",
                table: "VideoRequests",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AnswerJobID",
                table: "VideoRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
