using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SRIndia.Migrations
{
    public partial class _v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyMessages_Messages_MessageId",
                table: "ReplyMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReplyMessages",
                table: "ReplyMessages");

            migrationBuilder.RenameTable(
                name: "ReplyMessages",
                newName: "MessageReply");

            migrationBuilder.RenameIndex(
                name: "IX_ReplyMessages_MessageId",
                table: "MessageReply",
                newName: "IX_MessageReply_MessageId");

            migrationBuilder.AlterColumn<string>(
                name: "ReplyUserId",
                table: "MessageReply",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageReply",
                table: "MessageReply",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReply_Messages_MessageId",
                table: "MessageReply",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReply_Messages_MessageId",
                table: "MessageReply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageReply",
                table: "MessageReply");

            migrationBuilder.RenameTable(
                name: "MessageReply",
                newName: "ReplyMessages");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReply_MessageId",
                table: "ReplyMessages",
                newName: "IX_ReplyMessages_MessageId");

            migrationBuilder.AlterColumn<string>(
                name: "ReplyUserId",
                table: "ReplyMessages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReplyMessages",
                table: "ReplyMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyMessages_Messages_MessageId",
                table: "ReplyMessages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
