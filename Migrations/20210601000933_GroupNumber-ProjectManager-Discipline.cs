using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class GroupNumberProjectManagerDiscipline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisciplineId",
                table: "CourseProjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId",
                table: "CourseProjects",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupNumber",
                table: "AspNetUsers",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discipline",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discipline", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseProjects_DisciplineId",
                table: "CourseProjects",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProjects_ProjectManagerId",
                table: "CourseProjects",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseProjects_AspNetUsers_ProjectManagerId",
                table: "CourseProjects",
                column: "ProjectManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseProjects_Discipline_DisciplineId",
                table: "CourseProjects",
                column: "DisciplineId",
                principalTable: "Discipline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseProjects_AspNetUsers_ProjectManagerId",
                table: "CourseProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseProjects_Discipline_DisciplineId",
                table: "CourseProjects");

            migrationBuilder.DropTable(
                name: "Discipline");

            migrationBuilder.DropIndex(
                name: "IX_CourseProjects_DisciplineId",
                table: "CourseProjects");

            migrationBuilder.DropIndex(
                name: "IX_CourseProjects_ProjectManagerId",
                table: "CourseProjects");

            migrationBuilder.DropColumn(
                name: "DisciplineId",
                table: "CourseProjects");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "CourseProjects");

            migrationBuilder.DropColumn(
                name: "GroupNumber",
                table: "AspNetUsers");
        }
    }
}
