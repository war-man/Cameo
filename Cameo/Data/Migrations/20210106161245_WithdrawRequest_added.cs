using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class WithdrawRequest_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCancelled",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSuccess",
                table: "Invoices",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WithdrawRequestStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawRequestStatuses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WithdrawRequestStatuses_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WithdrawRequestStatuses_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawRequests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    StatusID = table.Column<int>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WithdrawRequests_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WithdrawRequests_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WithdrawRequests_WithdrawRequestStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "WithdrawRequestStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_CreatedBy",
                table: "WithdrawRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_ModifiedBy",
                table: "WithdrawRequests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_StatusID",
                table: "WithdrawRequests",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequestStatuses_CreatedBy",
                table: "WithdrawRequestStatuses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequestStatuses_ModifiedBy",
                table: "WithdrawRequestStatuses",
                column: "ModifiedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawRequests");

            migrationBuilder.DropTable(
                name: "WithdrawRequestStatuses");

            migrationBuilder.DropColumn(
                name: "DateCancelled",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DateSuccess",
                table: "Invoices");
        }
    }
}
