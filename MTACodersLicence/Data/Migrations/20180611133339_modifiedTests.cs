using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class modifiedTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<float>(
                name: "ExecutionTime",
                table: "TestResults",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Memory",
                table: "TestResults",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionTimeLimit",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ExecutionTime",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "Memory",
                table: "TestResults");
        }
    }
}
