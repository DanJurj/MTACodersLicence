using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modifyResult2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatteryId",
                table: "Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Results_BatteryId",
                table: "Results",
                column: "BatteryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Batteries_BatteryId",
                table: "Results",
                column: "BatteryId",
                principalTable: "Batteries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Batteries_BatteryId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_BatteryId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "BatteryId",
                table: "Results");
        }
    }
}
