using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class TalentProjectAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TalentProjects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    TalentID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentProjects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TalentProjects_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalentProjects_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalentProjects_Talents_TalentID",
                        column: x => x.TalentID,
                        principalTable: "Talents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalentProjects_CreatedBy",
                table: "TalentProjects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TalentProjects_ModifiedBy",
                table: "TalentProjects",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TalentProjects_TalentID",
                table: "TalentProjects",
                column: "TalentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalentProjects");
        }
    }
}
