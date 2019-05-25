using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cameo.Data.Migrations
{
    public partial class BaseInterfacesAndClassesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Posts",
                newName: "AuthorID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                newName: "IX_Posts_AuthorID");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Posts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorID",
                table: "Posts",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Posts",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AuthorID",
                table: "Posts",
                newName: "IX_Posts_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
