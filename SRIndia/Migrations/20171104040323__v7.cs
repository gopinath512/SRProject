using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SRIndia.Migrations
{
    public partial class _v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "AvatarImgId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileBGImgId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageImages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ImgId = table.Column<string>(nullable: false),
                    MessageId = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageImages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageImages_MessageId",
                table: "MessageImages",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageImages");

            migrationBuilder.DropColumn(
                name: "AvatarImgId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileBGImgId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ImgId",
                table: "Messages",
                nullable: false,
                defaultValue: "");
        }
    }
}
