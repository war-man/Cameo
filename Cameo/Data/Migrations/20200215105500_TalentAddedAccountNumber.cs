using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class TalentAddedAccountNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Talents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Talents");
        }
    }
}
