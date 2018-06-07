using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class addProgToSol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramingLanguages");

            migrationBuilder.AddColumn<int>(
                name: "ProgrammingLanguageId",
                table: "Solutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Available = table.Column<bool>(nullable: false),
                    CodeTemplate = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RankingViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Grade = table.Column<float>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    SentBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_ProgrammingLanguageId",
                table: "Solutions",
                column: "ProgrammingLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Solutions",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Solutions");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguages");

            migrationBuilder.DropTable(
                name: "RankingViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_ProgrammingLanguageId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "ProgrammingLanguageId",
                table: "Solutions");

            migrationBuilder.CreateTable(
                name: "ProgramingLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Available = table.Column<bool>(nullable: false),
                    CodeTemplate = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramingLanguages", x => x.Id);
                });
        }
    }
}
