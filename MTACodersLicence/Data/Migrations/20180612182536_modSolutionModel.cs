using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modSolutionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Memory",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Solutions",
                newName: "MemoryUsed");

            migrationBuilder.AddColumn<decimal>(
                name: "ExecutionTime",
                table: "Solutions",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionTime",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "MemoryUsed",
                table: "Solutions",
                newName: "Time");

            migrationBuilder.AddColumn<float>(
                name: "Memory",
                table: "Solutions",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
