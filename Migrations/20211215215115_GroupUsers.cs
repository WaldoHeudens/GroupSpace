using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupSpace.Migrations
{
    public partial class GroupUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualGroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Left = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BecameHost = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoHostAnymore = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActualGroupId",
                table: "AspNetUsers",
                column: "ActualGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_GroupId",
                table: "UserGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_UserId",
                table: "UserGroup",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Group_ActualGroupId",
                table: "AspNetUsers",
                column: "ActualGroupId",
                principalTable: "Group",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Group_ActualGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActualGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ActualGroupId",
                table: "AspNetUsers");
        }
    }
}
