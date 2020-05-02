using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class TalentModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntroVideoID",
                table: "Talents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Talents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_IntroVideoID",
                table: "Talents",
                column: "IntroVideoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_Attachments_IntroVideoID",
                table: "Talents",
                column: "IntroVideoID",
                principalTable: "Attachments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talents_Attachments_IntroVideoID",
                table: "Talents");

            migrationBuilder.DropIndex(
                name: "IX_Talents_IntroVideoID",
                table: "Talents");

            migrationBuilder.DropColumn(
                name: "IntroVideoID",
                table: "Talents");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Talents");
        }
    }
}
