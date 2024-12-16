using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVibes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingGroupProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudyPlan",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudyPlan",
                table: "Groups");
        }
    }
}
