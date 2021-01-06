using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_1.Migrations
{
    public partial class User_Table_with_Relation_to_Character : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterUserUserId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterUserUserId",
                table: "Characters",
                column: "CharacterUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_CharacterUserUserId",
                table: "Characters",
                column: "CharacterUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_CharacterUserUserId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterUserUserId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CharacterUserUserId",
                table: "Characters");
        }
    }
}
