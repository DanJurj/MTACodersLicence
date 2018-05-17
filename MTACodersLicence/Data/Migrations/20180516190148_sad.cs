using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class sad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Challenges_ChallengeModelId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_ChallengeModelId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "ChallengeModelId",
                table: "Solutions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveDateTime",
                table: "Solutions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeSpent",
                table: "Solutions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_ChallengeId",
                table: "Solutions",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Challenges_ChallengeId",
                table: "Solutions",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Challenges_ChallengeId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_ChallengeId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "ReceiveDateTime",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "Solutions");

            migrationBuilder.AddColumn<int>(
                name: "ChallengeModelId",
                table: "Solutions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_ChallengeModelId",
                table: "Solutions",
                column: "ChallengeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Challenges_ChallengeModelId",
                table: "Solutions",
                column: "ChallengeModelId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
