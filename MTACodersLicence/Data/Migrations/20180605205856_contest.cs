using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class contest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContestId",
                table: "Challenges",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContestModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Time = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContestModel_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ContestId",
                table: "Challenges",
                column: "ContestId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestModel_ApplicationUserId",
                table: "ContestModel",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_ContestModel_ContestId",
                table: "Challenges",
                column: "ContestId",
                principalTable: "ContestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_ContestModel_ContestId",
                table: "Challenges");

            migrationBuilder.DropTable(
                name: "ContestModel");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_ContestId",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "ContestId",
                table: "Challenges");
        }
    }
}
