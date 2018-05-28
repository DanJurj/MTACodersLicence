using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class testResultsMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "TestResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_ResultId",
                table: "TestResults",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Results_ResultId",
                table: "TestResults",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Results_ResultId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_ResultId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "TestResults");
        }
    }
}
