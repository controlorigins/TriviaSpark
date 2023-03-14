using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriviaSpark.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyTypeToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Matches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionType",
                table: "Matches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "QuestionType",
                table: "Matches");
        }
    }
}
