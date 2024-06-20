using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPortfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Achievement",
                table: "Achievement");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ReceiveDate",
                table: "Projects",
                newName: "CreationDateTime");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Achievement",
                newName: "OwnerUsername");

            migrationBuilder.AddColumn<string>(
                name: "UserProfileModelUsername",
                table: "Likes",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Achievement",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Achievement",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Achievement",
                table: "Achievement",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserProfileModelUsername",
                table: "Likes",
                column: "UserProfileModelUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserProfileModelUsername",
                table: "Likes",
                column: "UserProfileModelUsername",
                principalTable: "Users",
                principalColumn: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserProfileModelUsername",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserProfileModelUsername",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Achievement",
                table: "Achievement");

            migrationBuilder.DropColumn(
                name: "UserProfileModelUsername",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Achievement");

            migrationBuilder.RenameColumn(
                name: "CreationDateTime",
                table: "Projects",
                newName: "ReceiveDate");

            migrationBuilder.RenameColumn(
                name: "OwnerUsername",
                table: "Achievement",
                newName: "OwnerId");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "Likes",
                table: "Users",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Achievement",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Achievement",
                table: "Achievement",
                column: "Title");
        }
    }
}
