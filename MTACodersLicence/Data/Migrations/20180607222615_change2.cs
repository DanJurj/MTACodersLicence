using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class change2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeModelId",
                table: "GroupChallenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Contests_ContestId",
                table: "GroupChallenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Groups_GroupId",
                table: "GroupChallenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChallenges",
                table: "GroupChallenges");

            migrationBuilder.RenameTable(
                name: "GroupChallenges",
                newName: "GroupContests");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_GroupId",
                table: "GroupContests",
                newName: "IX_GroupContests_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_ContestId",
                table: "GroupContests",
                newName: "IX_GroupContests_ContestId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_ChallengeModelId",
                table: "GroupContests",
                newName: "IX_GroupContests_ChallengeModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupContests",
                table: "GroupContests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupContests_Challenges_ChallengeModelId",
                table: "GroupContests",
                column: "ChallengeModelId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupContests_Contests_ContestId",
                table: "GroupContests",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupContests_Groups_GroupId",
                table: "GroupContests",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupContests_Challenges_ChallengeModelId",
                table: "GroupContests");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupContests_Contests_ContestId",
                table: "GroupContests");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupContests_Groups_GroupId",
                table: "GroupContests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupContests",
                table: "GroupContests");

            migrationBuilder.RenameTable(
                name: "GroupContests",
                newName: "GroupChallenges");

            migrationBuilder.RenameIndex(
                name: "IX_GroupContests_GroupId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupContests_ContestId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_ContestId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupContests_ChallengeModelId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_ChallengeModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChallenges",
                table: "GroupChallenges",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallenges_Groups_GroupId",
                table: "GroupChallenges",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
