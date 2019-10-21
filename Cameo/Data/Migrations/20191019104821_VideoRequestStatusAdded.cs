using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestStatusAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestStatusID",
                table: "VideoRequests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VideoRequestStatuses",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoRequestStatuses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VideoRequestStatuses_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoRequestStatuses_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_RequestStatusID",
                table: "VideoRequests",
                column: "RequestStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequestStatuses_CreatedBy",
                table: "VideoRequestStatuses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequestStatuses_ModifiedBy",
                table: "VideoRequestStatuses",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_VideoRequestStatuses_RequestStatusID",
                table: "VideoRequests",
                column: "RequestStatusID",
                principalTable: "VideoRequestStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_VideoRequestStatuses_RequestStatusID",
                table: "VideoRequests");

            migrationBuilder.DropTable(
                name: "VideoRequestStatuses");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_RequestStatusID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "RequestStatusID",
                table: "VideoRequests");
        }
    }
}
