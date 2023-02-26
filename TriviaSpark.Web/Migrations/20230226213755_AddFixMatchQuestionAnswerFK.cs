using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriviaSpark.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddFixMatchQuestionAnswerFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchAnswers_Matches_AnswerId",
                table: "MatchAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchAnswers_Matches_MatchId",
                table: "MatchAnswers",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchAnswers_Matches_MatchId",
                table: "MatchAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchAnswers_Matches_AnswerId",
                table: "MatchAnswers",
                column: "AnswerId",
                principalTable: "Matches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
