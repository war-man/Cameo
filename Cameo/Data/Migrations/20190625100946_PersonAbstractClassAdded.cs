using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class PersonAbstractClassAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SocialAreas_SocialAreaID",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_SocialAreaID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FollowersCount",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SocialAreaHandle",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SocialAreaID",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    SocialAreaID = table.Column<int>(nullable: false),
                    SocialAreaHandle = table.Column<string>(nullable: true),
                    FollowersCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Talents_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talents_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talents_SocialAreas_SocialAreaID",
                        column: x => x.SocialAreaID,
                        principalTable: "SocialAreas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Talents_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Talents_CreatedBy",
                table: "Talents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_ModifiedBy",
                table: "Talents",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_SocialAreaID",
                table: "Talents",
                column: "SocialAreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UserID",
                table: "Talents",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Customers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FollowersCount",
                table: "Customers",
                nullable: true);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
