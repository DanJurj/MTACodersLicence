using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class profKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Rankings",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExecutionTime",
                table: "Rankings",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "TotalMemoryUsed",
                table: "Rankings",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Rankings");

            migrationBuilder.DropColumn(
                name: "TotalExecutionTime",
                table: "Rankings");

            migrationBuilder.DropColumn(
                name: "TotalMemoryUsed",
                table: "Rankings");
        }
    }
}
