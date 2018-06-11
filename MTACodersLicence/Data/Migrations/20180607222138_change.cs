using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeId",
                table: "GroupChallenges");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "GroupChallenges",
                newName: "ContestId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_ChallengeId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_ContestId");

            migrationBuilder.AddColumn<int>(
                name: "ChallengeModelId",
                table: "GroupChallenges",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupChallenges_ChallengeModelId",
                table: "GroupChallenges",
                column: "ChallengeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeModelId",
                table: "GroupChallenges",
                column: "ChallengeModelId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallenges_Contests_ContestId",
                table: "GroupChallenges",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeModelId",
                table: "GroupChallenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Contests_ContestId",
                table: "GroupChallenges");

            migrationBuilder.DropIndex(
                name: "IX_GroupChallenges_ChallengeModelId",
                table: "GroupChallenges");

            migrationBuilder.DropColumn(
                name: "ChallengeModelId",
                table: "GroupChallenges");

            migrationBuilder.RenameColumn(
                name: "ContestId",
                table: "GroupChallenges",
                newName: "ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_ContestId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeId",
                table: "GroupChallenges",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
