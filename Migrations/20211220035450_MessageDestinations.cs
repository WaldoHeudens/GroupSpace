using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupSpace.Migrations
{
    public partial class MessageDestinations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Group_GroupID",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_GroupID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "MessageID",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageDestinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "nvarchar(450)", nullable: false),
                    Received = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Read = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageDestinations_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageDestinations_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Media_MessageID",
                table: "Media",
                column: "MessageID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDestinations_MessageId",
                table: "MessageDestinations",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Message_MessageID",
                table: "Media",
                column: "MessageID",
                principalTable: "Message",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Message_MessageID",
                table: "Media");

            migrationBuilder.DropTable(
                name: "MessageDestinations");

            migrationBuilder.DropIndex(
                name: "IX_Media_MessageID",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "MessageID",
                table: "Media");

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Message_GroupID",
                table: "Message",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Group_GroupID",
                table: "Message",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
