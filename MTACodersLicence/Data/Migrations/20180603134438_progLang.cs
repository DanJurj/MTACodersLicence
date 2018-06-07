using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication5.Data.Migrations
{
    public partial class progLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinGroupRequestModel_AspNetUsers_ApplicationUserId",
                table: "JoinGroupRequestModel");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinGroupRequestModel_Groups_GroupId",
                table: "JoinGroupRequestModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JoinGroupRequestModel",
                table: "JoinGroupRequestModel");

            migrationBuilder.RenameTable(
                name: "JoinGroupRequestModel",
                newName: "JoinGroupRequests");

            migrationBuilder.RenameIndex(
                name: "IX_JoinGroupRequestModel_GroupId",
                table: "JoinGroupRequests",
                newName: "IX_JoinGroupRequests_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinGroupRequestModel_ApplicationUserId",
                table: "JoinGroupRequests",
                newName: "IX_JoinGroupRequests_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JoinGroupRequests",
                table: "JoinGroupRequests",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProgramingLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Available = table.Column<bool>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramingLanguages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_JoinGroupRequests_AspNetUsers_ApplicationUserId",
                table: "JoinGroupRequests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinGroupRequests_Groups_GroupId",
                table: "JoinGroupRequests",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinGroupRequests_AspNetUsers_ApplicationUserId",
                table: "JoinGroupRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinGroupRequests_Groups_GroupId",
                table: "JoinGroupRequests");

            migrationBuilder.DropTable(
                name: "ProgramingLanguages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JoinGroupRequests",
                table: "JoinGroupRequests");

            migrationBuilder.RenameTable(
                name: "JoinGroupRequests",
                newName: "JoinGroupRequestModel");

            migrationBuilder.RenameIndex(
                name: "IX_JoinGroupRequests_GroupId",
                table: "JoinGroupRequestModel",
                newName: "IX_JoinGroupRequestModel_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinGroupRequests_ApplicationUserId",
                table: "JoinGroupRequestModel",
                newName: "IX_JoinGroupRequestModel_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JoinGroupRequestModel",
                table: "JoinGroupRequestModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinGroupRequestModel_AspNetUsers_ApplicationUserId",
                table: "JoinGroupRequestModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinGroupRequestModel_Groups_GroupId",
                table: "JoinGroupRequestModel",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
