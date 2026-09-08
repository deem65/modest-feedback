using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModestFeedback.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Decision",
                table: "Submissions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Submissions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decision",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Submissions");
        }
    }
}
