using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupSpace.Migrations
{
    public partial class GroupStartedById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndedById",
                table: "Group",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StartedById",
                table: "Group",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndedById",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "StartedById",
                table: "Group");
        }
    }
}
