using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class SocialAreaIDToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialAreaHandle",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocialAreaID",
                table: "Customers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SocialAreaID",
                table: "Customers",
                column: "SocialAreaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SocialAreas_SocialAreaID",
                table: "Customers",
                column: "SocialAreaID",
                principalTable: "SocialAreas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SocialAreas_SocialAreaID",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_SocialAreaID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SocialAreaHandle",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SocialAreaID",
                table: "Customers");
        }
    }
}
