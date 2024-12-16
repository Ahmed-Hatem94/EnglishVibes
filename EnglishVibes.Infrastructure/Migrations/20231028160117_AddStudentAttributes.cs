using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVibes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActiveStatus",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudyPlan",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudyPlan",
                table: "AspNetUsers");
        }
    }
}
