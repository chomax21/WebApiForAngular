using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Angular_2.Migrations
{
    public partial class AddPriorityValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ToDoLists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ToDoLists");
        }
    }
}
