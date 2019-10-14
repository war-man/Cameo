using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class VideoRequestTypeModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "To",
                table: "VideoRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "VideoRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "VideoRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeID",
                table: "VideoRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VideoRequestTypes",
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
                    table.PrimaryKey("PK_VideoRequestTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VideoRequestTypes_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoRequestTypes_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequests_TypeID",
                table: "VideoRequests",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequestTypes_CreatedBy",
                table: "VideoRequestTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRequestTypes_ModifiedBy",
                table: "VideoRequestTypes",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoRequests_VideoRequestTypes_TypeID",
                table: "VideoRequests",
                column: "TypeID",
                principalTable: "VideoRequestTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoRequests_VideoRequestTypes_TypeID",
                table: "VideoRequests");

            migrationBuilder.DropTable(
                name: "VideoRequestTypes");

            migrationBuilder.DropIndex(
                name: "IX_VideoRequests_TypeID",
                table: "VideoRequests");

            migrationBuilder.DropColumn(
                name: "TypeID",
                table: "VideoRequests");

            migrationBuilder.AlterColumn<string>(
                name: "To",
                table: "VideoRequests",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "VideoRequests",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "VideoRequests",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
