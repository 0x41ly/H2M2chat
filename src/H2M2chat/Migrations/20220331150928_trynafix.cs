using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H2M2chat.Migrations
{
    public partial class trynafix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_CommentId1",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_CommentId1",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CommentId1",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Topic",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Topic",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Creator",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId1",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentId1",
                table: "Comment",
                column: "CommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_CommentId1",
                table: "Comment",
                column: "CommentId1",
                principalTable: "Comment",
                principalColumn: "CommentId");
        }
    }
}
