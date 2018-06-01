using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class renameGroupChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallengeModel_Challenges_ChallengeId",
                table: "GroupChallengeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallengeModel_Groups_GroupId",
                table: "GroupChallengeModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChallengeModel",
                table: "GroupChallengeModel");

            migrationBuilder.RenameTable(
                name: "GroupChallengeModel",
                newName: "GroupChallenges");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallengeModel_GroupId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallengeModel_ChallengeId",
                table: "GroupChallenges",
                newName: "IX_GroupChallenges_ChallengeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChallenges",
                table: "GroupChallenges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeId",
                table: "GroupChallenges",
                column: "ChallengeId",
                principalTable: "Challenges",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Challenges_ChallengeId",
                table: "GroupChallenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChallenges_Groups_GroupId",
                table: "GroupChallenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChallenges",
                table: "GroupChallenges");

            migrationBuilder.RenameTable(
                name: "GroupChallenges",
                newName: "GroupChallengeModel");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_GroupId",
                table: "GroupChallengeModel",
                newName: "IX_GroupChallengeModel_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChallenges_ChallengeId",
                table: "GroupChallengeModel",
                newName: "IX_GroupChallengeModel_ChallengeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChallengeModel",
                table: "GroupChallengeModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallengeModel_Challenges_ChallengeId",
                table: "GroupChallengeModel",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChallengeModel_Groups_GroupId",
                table: "GroupChallengeModel",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
