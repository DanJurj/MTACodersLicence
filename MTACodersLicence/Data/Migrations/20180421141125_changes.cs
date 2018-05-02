using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeModel_AspNetUsers_ApplicationUserId",
                table: "ChallengeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupItem_Groups_GroupId",
                table: "GroupItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TestModel_ChallengeModel_ChallengeId",
                table: "TestModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestModel",
                table: "TestModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupItem",
                table: "GroupItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChallengeModel",
                table: "ChallengeModel");

            migrationBuilder.RenameTable(
                name: "TestModel",
                newName: "Tests");

            migrationBuilder.RenameTable(
                name: "GroupItem",
                newName: "GroupItems");

            migrationBuilder.RenameTable(
                name: "ChallengeModel",
                newName: "Challenges");

            migrationBuilder.RenameIndex(
                name: "IX_TestModel_ChallengeId",
                table: "Tests",
                newName: "IX_Tests_ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupItem_GroupId",
                table: "GroupItems",
                newName: "IX_GroupItems_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ChallengeModel_ApplicationUserId",
                table: "Challenges",
                newName: "IX_Challenges_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tests",
                table: "Tests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupItems",
                table: "GroupItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Challenges",
                table: "Challenges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_AspNetUsers_ApplicationUserId",
                table: "Challenges",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupItems_Groups_GroupId",
                table: "GroupItems",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Challenges_ChallengeId",
                table: "Tests",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_AspNetUsers_ApplicationUserId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupItems_Groups_GroupId",
                table: "GroupItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Challenges_ChallengeId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tests",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupItems",
                table: "GroupItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Challenges",
                table: "Challenges");

            migrationBuilder.RenameTable(
                name: "Tests",
                newName: "TestModel");

            migrationBuilder.RenameTable(
                name: "GroupItems",
                newName: "GroupItem");

            migrationBuilder.RenameTable(
                name: "Challenges",
                newName: "ChallengeModel");

            migrationBuilder.RenameIndex(
                name: "IX_Tests_ChallengeId",
                table: "TestModel",
                newName: "IX_TestModel_ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupItems_GroupId",
                table: "GroupItem",
                newName: "IX_GroupItem_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Challenges_ApplicationUserId",
                table: "ChallengeModel",
                newName: "IX_ChallengeModel_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestModel",
                table: "TestModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupItem",
                table: "GroupItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChallengeModel",
                table: "ChallengeModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeModel_AspNetUsers_ApplicationUserId",
                table: "ChallengeModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupItem_Groups_GroupId",
                table: "GroupItem",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestModel_ChallengeModel_ChallengeId",
                table: "TestModel",
                column: "ChallengeId",
                principalTable: "ChallengeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
