using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class addProgToCodSes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgrammingLanguageId",
                table: "CodingSessions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CodingSessions_ProgrammingLanguageId",
                table: "CodingSessions",
                column: "ProgrammingLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingSessions_ProgrammingLanguages_ProgrammingLanguageId",
                table: "CodingSessions",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingSessions_ProgrammingLanguages_ProgrammingLanguageId",
                table: "CodingSessions");

            migrationBuilder.DropIndex(
                name: "IX_CodingSessions_ProgrammingLanguageId",
                table: "CodingSessions");

            migrationBuilder.DropColumn(
                name: "ProgrammingLanguageId",
                table: "CodingSessions");
        }
    }
}
