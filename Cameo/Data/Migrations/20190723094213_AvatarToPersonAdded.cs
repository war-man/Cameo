using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class AvatarToPersonAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarID",
                table: "Talents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvatarID",
                table: "Customers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_AvatarID",
                table: "Talents",
                column: "AvatarID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AvatarID",
                table: "Customers",
                column: "AvatarID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Attachments_AvatarID",
                table: "Customers",
                column: "AvatarID",
                principalTable: "Attachments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_Attachments_AvatarID",
                table: "Talents",
                column: "AvatarID",
                principalTable: "Attachments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Attachments_AvatarID",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Talents_Attachments_AvatarID",
                table: "Talents");

            migrationBuilder.DropIndex(
                name: "IX_Talents_AvatarID",
                table: "Talents");

            migrationBuilder.DropIndex(
                name: "IX_Customers_AvatarID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AvatarID",
                table: "Talents");

            migrationBuilder.DropColumn(
                name: "AvatarID",
                table: "Customers");
        }
    }
}
