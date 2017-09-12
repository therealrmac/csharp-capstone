using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatItUp.Data.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AspNetUsers_friendId",
                table: "Relation");

            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AspNetUsers_userId",
                table: "Relation");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Relation",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "friendId",
                table: "Relation",
                newName: "FriendId");

            migrationBuilder.RenameColumn(
                name: "connectedOn",
                table: "Relation",
                newName: "ConnectedOn");

            migrationBuilder.RenameColumn(
                name: "connected",
                table: "Relation",
                newName: "Connected");

            migrationBuilder.RenameIndex(
                name: "IX_Relation_userId",
                table: "Relation",
                newName: "IX_Relation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Relation_friendId",
                table: "Relation",
                newName: "IX_Relation_FriendId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Thread",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorMessage",
                table: "Thread",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AspNetUsers_FriendId",
                table: "Relation",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AspNetUsers_UserId",
                table: "Relation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AspNetUsers_FriendId",
                table: "Relation");

            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AspNetUsers_UserId",
                table: "Relation");

            migrationBuilder.DropColumn(
                name: "AuthorMessage",
                table: "Thread");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Relation",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "Relation",
                newName: "friendId");

            migrationBuilder.RenameColumn(
                name: "ConnectedOn",
                table: "Relation",
                newName: "connectedOn");

            migrationBuilder.RenameColumn(
                name: "Connected",
                table: "Relation",
                newName: "connected");

            migrationBuilder.RenameIndex(
                name: "IX_Relation_UserId",
                table: "Relation",
                newName: "IX_Relation_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Relation_FriendId",
                table: "Relation",
                newName: "IX_Relation_friendId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Thread",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AspNetUsers_friendId",
                table: "Relation",
                column: "friendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AspNetUsers_userId",
                table: "Relation",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
