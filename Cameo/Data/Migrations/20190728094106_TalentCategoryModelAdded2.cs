using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class TalentCategoryModelAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TalentCategories",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    TalentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TalentCategories_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalentCategories_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalentCategories_Talents_TalentID",
                        column: x => x.TalentID,
                        principalTable: "Talents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalentCategories_CreatedBy",
                table: "TalentCategories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TalentCategories_ModifiedBy",
                table: "TalentCategories",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TalentCategories_TalentID",
                table: "TalentCategories",
                column: "TalentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalentCategories");
        }
    }
}
