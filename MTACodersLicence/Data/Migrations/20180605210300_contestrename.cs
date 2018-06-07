using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class contestrename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_ContestModel_ContestId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_ContestModel_AspNetUsers_ApplicationUserId",
                table: "ContestModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RankingViewModel",
                table: "RankingViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContestModel",
                table: "ContestModel");

            migrationBuilder.RenameTable(
                name: "RankingViewModel",
                newName: "Rankings");

            migrationBuilder.RenameTable(
                name: "ContestModel",
                newName: "Contests");

            migrationBuilder.RenameIndex(
                name: "IX_ContestModel_ApplicationUserId",
                table: "Contests",
                newName: "IX_Contests_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rankings",
                table: "Rankings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contests",
                table: "Contests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Contests_ContestId",
                table: "Challenges",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_AspNetUsers_ApplicationUserId",
                table: "Contests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Contests_ContestId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Contests_AspNetUsers_ApplicationUserId",
                table: "Contests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rankings",
                table: "Rankings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contests",
                table: "Contests");

            migrationBuilder.RenameTable(
                name: "Rankings",
                newName: "RankingViewModel");

            migrationBuilder.RenameTable(
                name: "Contests",
                newName: "ContestModel");

            migrationBuilder.RenameIndex(
                name: "IX_Contests_ApplicationUserId",
                table: "ContestModel",
                newName: "IX_ContestModel_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RankingViewModel",
                table: "RankingViewModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContestModel",
                table: "ContestModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_ContestModel_ContestId",
                table: "Challenges",
                column: "ContestId",
                principalTable: "ContestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContestModel_AspNetUsers_ApplicationUserId",
                table: "ContestModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
