using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class BaseModelDropdownableClassAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talents_SocialAreas_SocialAreaID",
                table: "Talents");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SocialAreas",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "SocialAreaID",
                table: "Talents",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "FollowersCount",
                table: "Talents",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_SocialAreas_SocialAreaID",
                table: "Talents",
                column: "SocialAreaID",
                principalTable: "SocialAreas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talents_SocialAreas_SocialAreaID",
                table: "Talents");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SocialAreas",
                newName: "Title");

            migrationBuilder.AlterColumn<int>(
                name: "SocialAreaID",
                table: "Talents",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FollowersCount",
                table: "Talents",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_SocialAreas_SocialAreaID",
                table: "Talents",
                column: "SocialAreaID",
                principalTable: "SocialAreas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
