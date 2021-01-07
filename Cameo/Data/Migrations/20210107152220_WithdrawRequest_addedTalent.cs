using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class WithdrawRequest_addedTalent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TalentID",
                table: "WithdrawRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_TalentID",
                table: "WithdrawRequests",
                column: "TalentID");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawRequests_Talents_TalentID",
                table: "WithdrawRequests",
                column: "TalentID",
                principalTable: "Talents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawRequests_Talents_TalentID",
                table: "WithdrawRequests");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawRequests_TalentID",
                table: "WithdrawRequests");

            migrationBuilder.DropColumn(
                name: "TalentID",
                table: "WithdrawRequests");
        }
    }
}
