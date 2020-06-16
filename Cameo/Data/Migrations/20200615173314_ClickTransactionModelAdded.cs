using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class ClickTransactionModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClickTransactions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ClickTransID = table.Column<int>(nullable: false),
                    ServiceID = table.Column<int>(nullable: false),
                    ClickPaydocID = table.Column<int>(nullable: false),
                    MerchantTransID = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    Error = table.Column<int>(nullable: false),
                    ErrorNote = table.Column<string>(nullable: true),
                    SignTime = table.Column<string>(nullable: true),
                    SignString = table.Column<string>(nullable: true),
                    StatusID = table.Column<int>(nullable: false),
                    DateSuccess = table.Column<DateTime>(nullable: true),
                    DateCancelled = table.Column<DateTime>(nullable: true),
                    CustomerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickTransactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClickTransactions_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClickTransactions_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClickTransactions_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClickTransactions_CreatedBy",
                table: "ClickTransactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClickTransactions_CustomerID",
                table: "ClickTransactions",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ClickTransactions_ModifiedBy",
                table: "ClickTransactions",
                column: "ModifiedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClickTransactions");
        }
    }
}
