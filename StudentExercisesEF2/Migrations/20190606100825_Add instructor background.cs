using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentExercisesEF.Migrations
{
    public partial class Addinstructorbackground : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Background",
                table: "Instructor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Background",
                table: "Instructor");
        }
    }
}
