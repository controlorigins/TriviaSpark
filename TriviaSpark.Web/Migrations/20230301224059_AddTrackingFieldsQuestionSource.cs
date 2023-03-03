using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace TriviaSpark.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingFieldsQuestionSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "QuestionAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "QuestionAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MatchQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "MatchQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Matches",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Matches",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MatchAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "MatchAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MatchQuestions");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "MatchQuestions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MatchAnswers");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "MatchAnswers");
        }
    }
}
