using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class chageLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filename",
                table: "ProgrammingLanguages");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ProgrammingLanguages",
                newName: "EditorMode");

            migrationBuilder.AddColumn<int>(
                name: "LanguageCode",
                table: "ProgrammingLanguages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProgrammingLanguages");

            migrationBuilder.RenameColumn(
                name: "EditorMode",
                table: "ProgrammingLanguages",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "ProgrammingLanguages",
                nullable: true);
        }
    }
}
