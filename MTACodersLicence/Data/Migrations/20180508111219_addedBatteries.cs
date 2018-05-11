using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class addedBatteries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Challenges_ChallengeId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_ChallengeId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "Tests");

            migrationBuilder.AddColumn<int>(
                name: "BatteryId",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Points",
                table: "Tests",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Tasks",
                table: "Challenges",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BatteryModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChallengeId = table.Column<int>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatteryModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatteryModel_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_BatteryId",
                table: "Tests",
                column: "BatteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BatteryModel_ChallengeId",
                table: "BatteryModel",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_BatteryModel_BatteryId",
                table: "Tests",
                column: "BatteryId",
                principalTable: "BatteryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_BatteryModel_BatteryId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "BatteryModel");

            migrationBuilder.DropIndex(
                name: "IX_Tests_BatteryId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "BatteryId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Tasks",
                table: "Challenges");

            migrationBuilder.AddColumn<int>(
                name: "ChallengeId",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ChallengeId",
                table: "Tests",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Challenges_ChallengeId",
                table: "Tests",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
