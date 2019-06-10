using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentExercisesEF.Migrations
{
    public partial class restrictdeletecohort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Cohort_CohortId",
                table: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Cohort_CohortId",
                table: "Student",
                column: "CohortId",
                principalTable: "Cohort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Cohort_CohortId",
                table: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Cohort_CohortId",
                table: "Student",
                column: "CohortId",
                principalTable: "Cohort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
