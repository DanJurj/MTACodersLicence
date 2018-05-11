using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class changeBatteryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatteryModel_Challenges_ChallengeId",
                table: "BatteryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_BatteryModel_BatteryId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BatteryModel",
                table: "BatteryModel");

            migrationBuilder.RenameTable(
                name: "BatteryModel",
                newName: "Batteries");

            migrationBuilder.RenameIndex(
                name: "IX_BatteryModel_ChallengeId",
                table: "Batteries",
                newName: "IX_Batteries_ChallengeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Batteries",
                table: "Batteries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Batteries_Challenges_ChallengeId",
                table: "Batteries",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Batteries_BatteryId",
                table: "Tests",
                column: "BatteryId",
                principalTable: "Batteries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batteries_Challenges_ChallengeId",
                table: "Batteries");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Batteries_BatteryId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Batteries",
                table: "Batteries");

            migrationBuilder.RenameTable(
                name: "Batteries",
                newName: "BatteryModel");

            migrationBuilder.RenameIndex(
                name: "IX_Batteries_ChallengeId",
                table: "BatteryModel",
                newName: "IX_BatteryModel_ChallengeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BatteryModel",
                table: "BatteryModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BatteryModel_Challenges_ChallengeId",
                table: "BatteryModel",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_BatteryModel_BatteryId",
                table: "Tests",
                column: "BatteryId",
                principalTable: "BatteryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
