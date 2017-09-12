using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatItUp.Data.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "dateCreatd",
                table: "ThreadPost",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created",
                table: "Thread",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Thread",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConnectedOn",
                table: "Relation",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateIndex(
                name: "IX_Thread_UserId",
                table: "Thread",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thread_AspNetUsers_UserId",
                table: "Thread",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thread_AspNetUsers_UserId",
                table: "Thread");

            migrationBuilder.DropIndex(
                name: "IX_Thread_UserId",
                table: "Thread");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Thread");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateCreatd",
                table: "ThreadPost",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created",
                table: "Thread",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConnectedOn",
                table: "Relation",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
