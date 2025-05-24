using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserKeyToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserKey",
                table: "Feedbacks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserKey",
                table: "Feedbacks");
        }
    }
}
