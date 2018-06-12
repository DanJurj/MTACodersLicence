using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modContests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Challenges");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Contests",
                newName: "Duration");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Contests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Contests");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Contests",
                newName: "Time");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Challenges",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Challenges",
                nullable: false,
                defaultValue: 0);
        }
    }
}
