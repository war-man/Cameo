using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class CreditCardFieldsAddedToTalent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreditCardExpire",
                table: "Talents",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditCardNumber",
                table: "Talents",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditCardExpire",
                table: "Talents");

            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "Talents");
        }
    }
}
