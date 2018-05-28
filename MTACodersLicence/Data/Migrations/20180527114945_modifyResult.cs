using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modifyResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_ApplicationUserId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Batteries_BatteryId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Challenges_ChallengeId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_ApplicationUserId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_BatteryId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "BatteryId",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "Results",
                newName: "SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_ChallengeId",
                table: "Results",
                newName: "IX_Results_SolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Solutions_SolutionId",
                table: "Results",
                column: "SolutionId",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Solutions_SolutionId",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "SolutionId",
                table: "Results",
                newName: "ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_SolutionId",
                table: "Results",
                newName: "IX_Results_ChallengeId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Results",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BatteryId",
                table: "Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Results_ApplicationUserId",
                table: "Results",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_BatteryId",
                table: "Results",
                column: "BatteryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_ApplicationUserId",
                table: "Results",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Batteries_BatteryId",
                table: "Results",
                column: "BatteryId",
                principalTable: "Batteries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Challenges_ChallengeId",
                table: "Results",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
