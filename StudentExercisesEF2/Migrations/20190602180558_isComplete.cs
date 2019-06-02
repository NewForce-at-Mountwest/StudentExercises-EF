using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentExercisesEF.Migrations
{
    public partial class isComplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isComplete",
                table: "StudentExercise",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isComplete",
                table: "StudentExercise");
        }
    }
}
