using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modSol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_AspNetUsers_ApplicationUserId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_ApplicationUserId",
                table: "Solutions");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Solutions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Solutions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_ApplicationUserId",
                table: "Solutions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_AspNetUsers_ApplicationUserId",
                table: "Solutions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
