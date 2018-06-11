using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modifiedTests2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionTimeLimit",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "Tests");

            migrationBuilder.AddColumn<float>(
                name: "ExecutionTimeLimit",
                table: "Challenges",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MemoryLimit",
                table: "Challenges",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionTimeLimit",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "Challenges");

            migrationBuilder.AddColumn<float>(
                name: "ExecutionTimeLimit",
                table: "Tests",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MemoryLimit",
                table: "Tests",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
